using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
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
         Location origin = HONGKONG;
         Location destination = STOCKHOLM;         
         DateTime arrivalDeadline = new DateTime(2009, 3, 18);

         /* Use case 1: booking

            A new cargo is booked, and the unique tracking id is assigned to the cargo. */
         TrackingId trackingId = BookingService.BookNewCargo(
            origin.UnLocode, destination.UnLocode, arrivalDeadline
            );

         /* The tracking id can be used to lookup the cargo in the repository.

            Important: The cargo, and thus the domain model, is responsible for determining
            the status of the cargo, whether it is on the right track or not and so on.
            This is core domain logic.

            Tracking the cargo basically amounts to presenting information extracted from
            the cargo aggregate in a suitable way. */
         Cargo cargo = CargoRepository.Find(trackingId);
         Assert.IsNotNull(cargo);         

         /* Use case 2: routing

            A number of possible routes for this cargo is requested and may be
            presented to the customer in some way for him/her to choose from.
            Selection could be affected by things like price and time of delivery,
            but this test simply uses an arbitrary selection to mimic that process.

            The cargo is then assigned to the selected route, described by an itinerary. */
         IList<Itinerary> itineraries = BookingService.RequestPossibleRoutesForCargo(trackingId);
         Itinerary itinerary = SelectPreferedItinerary(itineraries);

         using (DomainEvents.Register<CargoWasAssignedToRouteEvent>(x =>
                                         {
                                            Assert.AreEqual(TransportStatus.NotReceived, x.Delivery.TransportStatus);
                                            Assert.AreEqual(RoutingStatus.Routed, x.Delivery.RoutingStatus);
                                            Assert.IsNotNull(x.Delivery.EstimatedTimeOfArrival);
                                            Assert.AreEqual(new HandlingActivity(HandlingEventType.Receive, HONGKONG), x.Delivery.NextExpectedActivity);
                                         }))
         {
            BookingService.AssignCargoToRoute(trackingId, itinerary);            
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
         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.AreEqual(HONGKONG, x.Delivery.LastKnownLocation);
                                                               }))
         {
            HandlingEventService.RegisterHandlingEvent(
               new DateTime(2009, 3, 1), trackingId, HONGKONG.UnLocode, HandlingEventType.Receive
               );   
         }

         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
         {
            // Check current state - should be ok            
            Assert.AreEqual(HONGKONG, x.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.OnboardCarrier, x.Delivery.TransportStatus);
            Assert.IsFalse(x.Delivery.IsMisdirected);
            Assert.AreEqual(new HandlingActivity(HandlingEventType.Unload, NEWYORK), x.Delivery.NextExpectedActivity);
         }))
         {
            // Next event: Load onto voyage CM003 in Hongkong
            HandlingEventService.RegisterHandlingEvent(
               new DateTime(2009, 3, 3), trackingId, HONGKONG.UnLocode, HandlingEventType.Load
               );
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

         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  // Check current state - cargo is misdirected!
                                                                  Assert.AreEqual(TOKYO, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.IsTrue(x.Delivery.IsMisdirected);
                                                                  Assert.IsNull(x.Delivery.NextExpectedActivity);
                                                               }))
         {
            // Cargo is now (incorrectly) unloaded in Tokyo
            HandlingEventService.RegisterHandlingEvent(
               new DateTime(2009, 3, 5), trackingId, TOKYO.UnLocode, HandlingEventType.Unload
               );
         }

         // -- Cargo needs to be rerouted --

         // TODO cleaner reroute from "earliest location from where the new route originates"

         // Specify a new route, this time to Goeteborg.
         using (DomainEvents.Register<CargoDestinationChangedEvent>(x =>
                                                               {
                                                                  // The old itinerary does not satisfy the new specification
                                                                  Assert.AreEqual(RoutingStatus.Misrouted, x.Delivery.RoutingStatus);
                                                                  Assert.IsNull(x.Delivery.NextExpectedActivity);
                                                               }))
         {
            cargo.SpecifyNewRoute(GOETEBORG);
         }
         
         // Repeat procedure of selecting one out of a number of possible routes satisfying the route spec
         IList<Itinerary> newItineraries = BookingService.RequestPossibleRoutesForCargo(cargo.TrackingId);
         Itinerary newItinerary = SelectPreferedItinerary(newItineraries);

         using (DomainEvents.Register<CargoWasAssignedToRouteEvent>(x =>
                                                                       {
                                                                          // New itinerary should satisfy new route
                                                                          Assert.AreEqual(RoutingStatus.Routed, x.Delivery.RoutingStatus);
                                                                          Assert.IsTrue(x.Delivery.IsMisdirected);                                                                          
                                                                       }))
         {
            BookingService.AssignCargoToRoute(trackingId, newItinerary);            
         }
         
         // -- Cargo has been rerouted, shipping continues --


         // Load in Tokyo
         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(HONGKONG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.OnboardCarrier, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Unload, HAMBURG), x.Delivery.NextExpectedActivity);  
                                                               }))
         {
            HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 8), trackingId, HONGKONG.UnLocode,
                                                       HandlingEventType.Load);
         }
         
         // Unload in Hamburg
         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(HAMBURG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Load, HAMBURG), x.Delivery.NextExpectedActivity);
                                                               }))
         {
            HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 12), trackingId, HAMBURG.UnLocode,
                                                       HandlingEventType.Unload);
         }         

         // Load in Hamburg
         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(HAMBURG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.OnboardCarrier, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Unload, GOETEBORG), x.Delivery.NextExpectedActivity);
                                                               }))
         {
            HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 14), trackingId, HAMBURG.UnLocode,
                                                       HandlingEventType.Load);
         }         

         // Unload in Stockholm
         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(GOETEBORG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.InPort, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.AreEqual(new HandlingActivity(HandlingEventType.Claim, GOETEBORG), x.Delivery.NextExpectedActivity);
                                                               }))
         {
            HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 15), trackingId, GOETEBORG.UnLocode,
                                                       HandlingEventType.Unload);
         }
         

         // Finally, cargo is claimed in Stockholm. This ends the cargo lifecycle from our perspective.
         using (DomainEvents.Register<CargoWasHandledEvent>(x =>
                                                               {
                                                                  // Check current state - should be ok
                                                                  Assert.AreEqual(GOETEBORG, x.Delivery.LastKnownLocation);
                                                                  Assert.AreEqual(TransportStatus.Claimed, x.Delivery.TransportStatus);
                                                                  Assert.IsFalse(x.Delivery.IsMisdirected);
                                                                  Assert.IsNull(x.Delivery.NextExpectedActivity);
                                                               }))
         {
            HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 16), trackingId, GOETEBORG.UnLocode,
                                                       HandlingEventType.Claim);
         }
         
      }
      private static Itinerary SelectPreferedItinerary(IList<Itinerary> itineraries)
      {
         return itineraries.First();
      }      
   }
}