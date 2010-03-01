using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Commands;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using Microsoft.Practices.ServiceLocation;
using NServiceBus;
using NUnit.Framework;

namespace Tests.Integration
{
   [TestFixture]
   public class FromHongkongToStockholm : ScenarioTest
   {
      [Test]
      public void CanTransportFromHongkongToStockholm()
      {
         /* Test setup: A cargo should be shipped from Hongkong to Stockholm,
            and it should arrive in no more than two weeks. */
         UnLocode origin = HONGKONG;
         UnLocode destination = STOCKHOLM;         
         DateTime arrivalDeadline = new DateTime(2009, 3, 18);

         /* Use case 1: booking

            A new cargo is booked, and the unique tracking id is assigned to the cargo. */
         Guid cargoId = Guid.Empty;
         using (Bus.Register<Cargo, CargoRegisteredEvent>((s,x) => cargoId = s.Id))
         {
            InvokeCommand(new BookNewCargoCommand
                             {
                                Origin = origin.CodeString,
                                Destination = destination.CodeString,
                                ArrivalDeadline = arrivalDeadline
                             });
         }


         List<LegDTO> itinerary
            = new List<LegDTO>
                 {
                    new LegDTO(HONGKONG.CodeString, new DateTime(2009, 3, 03),
                               NEWYORK.CodeString, new DateTime(2009, 3, 9)),
                    new LegDTO(NEWYORK.CodeString, new DateTime(2009, 3, 10),
                            CHICAGO.CodeString, new DateTime(2009, 3, 14)),
                    new LegDTO(CHICAGO.CodeString, new DateTime(2009, 3, 7),
                            STOCKHOLM.CodeString, new DateTime(2009, 3, 11))
                 };

         using (Bus.Register<Cargo, CargoAssignedToRouteEvent>((s,x) =>
                                         {
                                            Assert.AreEqual(TransportStatus.NotReceived, x.Delivery.TransportStatus);
                                            Assert.AreEqual(RoutingStatus.Routed, x.Delivery.RoutingStatus);
                                            Assert.IsNotNull(x.Delivery.EstimatedTimeOfArrival);
                                            Assert.AreEqual(new HandlingActivity(HandlingEventType.Receive, HONGKONG), x.Delivery.NextExpectedActivity);
                                         }))
         {
            InvokeCommand(new AssignCargoToRouteCommand()
                             {
                                CargoId = cargoId,
                                Legs = itinerary
                             });
         }
         
         /*
           Use case 3: handling

           A handling event registration attempt will be formed from parsing
           the data coming in as a handling report either via
           the web service interface or as an uploaded CSV file.

           The handling event factory tries to create a HandlingEvent from the attempt,
           and if the factory decides that this is a plausible handling event, it is stored.
           If the attempt is invalid, for example if no cargo exists for the specfied tracking id,
           the attempt is rejected.

           Handling begins: cargo is received in Hongkong.
           */
         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.AreEqual(HONGKONG, x.Delivery.LastKnownLocation);
                                                               }))
         {
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 1), HONGKONG, HandlingEventType.Receive);   
         }

         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
         {
            // Check current state - should be ok            
            Assert.AreEqual(HONGKONG, x.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.OnboardCarrier, x.Delivery.TransportStatus);
            Assert.IsFalse(x.Delivery.IsMisdirected);
            Assert.AreEqual(new HandlingActivity(HandlingEventType.Unload, NEWYORK), x.Delivery.NextExpectedActivity);
         }))
         {
            // Next event: Load onto voyage CM003 in Hongkong
            RegisterHandlingEvent(cargoId,new DateTime(2009, 3, 3), HONGKONG, HandlingEventType.Load);
         }                   


         /*
           Here's an attempt to register a handling event that's not valid
           because there is no voyage with the specified voyage number,
           and there's no location with the specified UN Locode either.

           This attempt will be rejected and will not affect the cargo delivery in any way.
          */
         //VoyageNumber noSuchVoyageNumber = new VoyageNumber("XX000");
         //UnLocode noSuchUnLocode = new UnLocode("ZZZZZ");
         //try
         //{
         //   HandlingEventService.RegisterHandlingEvent(
         //   new DateTime(2009, 3, 5), trackingId, noSuchUnLocode, HandlingEventType.Load
         //   );
         //   Assert.Fail("Should not be able to register a handling event with invalid location and voyage");
         //}
         //catch (ArgumentException)
         //{
         //}

         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  // Check current state - cargo is misdirected!
                                                                  Assert.AreEqual(TOKYO, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.IsTrue(x.Delivery.IsMisdirected);
                                                                  Assert.IsNull(x.Delivery.NextExpectedActivity);
                                                               }))
         {
            // Cargo is now (incorrectly) unloaded in Tokyo
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 5), TOKYO, HandlingEventType.Unload);
         }

         // -- Cargo needs to be rerouted --

         // TODO cleaner reroute from "earliest location from where the new route originates"

         // Specify a new route, this time to Goeteborg.
         using (Bus.Register<Cargo, CargoDestinationChangedEvent>((s, x) =>
                                                               {
                                                                  // The old itinerary does not satisfy the new specification
                                                                  Assert.AreEqual(RoutingStatus.Misrouted, x.Delivery.RoutingStatus);
                                                                  Assert.IsNull(x.Delivery.NextExpectedActivity);
                                                               }))
         {
            InvokeCommand(new ChangeDestinationCommand
                             {
                                CargoId = cargoId,
                                NewDestination = GOETEBORG.CodeString
                             });
         }

         var newItinerary = new List<LegDTO>
                               {
                                  new LegDTO(HONGKONG.CodeString, new DateTime(2009, 3, 8),
                                             HAMBURG.CodeString, new DateTime(2009, 3, 12)),
                                  new LegDTO(HAMBURG.CodeString, new DateTime(2009, 3, 14),
                                             GOETEBORG.CodeString, new DateTime(2009, 3, 15))
                               };

         using (Bus.Register<Cargo, CargoAssignedToRouteEvent>((s, x) =>
                                                                       {
                                                                          // New itinerary should satisfy new route
                                                                          Assert.AreEqual(RoutingStatus.Routed, x.Delivery.RoutingStatus);
                                                                          Assert.IsTrue(x.Delivery.IsMisdirected);                                                                          
                                                                       }))
         {
            InvokeCommand(new AssignCargoToRouteCommand()
            {
               CargoId = cargoId,
               Legs = newItinerary
            });    
         }
         
         // -- Cargo has been rerouted, shipping continues --


         // Load in Tokyo
         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(HONGKONG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.OnboardCarrier, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Unload, HAMBURG), x.Delivery.NextExpectedActivity);  
                                                               }))
         {
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 8), HONGKONG, HandlingEventType.Load);
         }
         
         // Unload in Hamburg
         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(HAMBURG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Load, HAMBURG), x.Delivery.NextExpectedActivity);
                                                               }))
         {
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 12), HAMBURG, HandlingEventType.Unload);
         }         

         // Load in Hamburg
         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(HAMBURG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.OnboardCarrier, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Unload, GOETEBORG), x.Delivery.NextExpectedActivity);
                                                               }))
         {
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 14), HAMBURG, HandlingEventType.Load);
         }         

         // Unload in Stockholm
         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(GOETEBORG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Claim, GOETEBORG), x.Delivery.NextExpectedActivity);
                                                               }))
         {
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 15), GOETEBORG, HandlingEventType.Unload);
         }
         

         // Finally, cargo is claimed in Stockholm. This ends the cargo lifecycle from our perspective.
         using (Bus.Register<Cargo, CargoHandledEvent>((s, x) =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(GOETEBORG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.Claimed, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.IsNull(x.Delivery.NextExpectedActivity);
                                                               }))
         {
            RegisterHandlingEvent(cargoId, new DateTime(2009, 3, 16), GOETEBORG,
                                                       HandlingEventType.Claim);
         }
         
      }

      private static void RegisterHandlingEvent(Guid cargoId, DateTime time, UnLocode location, HandlingEventType eventType)
      {
         InvokeCommand(new RegisterHandlingEventCommand
                          {
                             CargoId = cargoId,
                             CompletionTime = time,
                             Location = location.CodeString,
                             Type = eventType
                          });
      }


      private static void InvokeCommand<T>(T command)
         where T : IMessage
      {
         UnitOfWork.Current = new UnitOfWork(_sessionFactory);
         IMessageHandler<T> handler = ServiceLocator.Current.GetInstance<IMessageHandler<T>>();
         handler.Handle(command);
         UnitOfWork.Current.Commit();
         UnitOfWork.Current = null;
      }
   }
}