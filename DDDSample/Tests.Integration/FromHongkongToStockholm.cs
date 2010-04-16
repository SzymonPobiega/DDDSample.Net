using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Persistence;
using DDDSample.DomainModel.Potential.Location;
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
         Location origin = SampleLocations.Hongkong;
         Location destination = SampleLocations.Stockholm;
         DateTime arrivalDeadline = DateTime.Now.AddDays(20);//new DateTime(2009, 3, 18);

         /* Use case 1: booking

            A new cargo is booked, and the unique tracking id is assigned to the cargo. */
         TrackingId trackingId = BookingService.BookNewCargo("c1",
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
         Assert.AreEqual(TransportStatus.NotReceived, cargo.Delivery.TransportStatus);
         Assert.AreEqual(RoutingStatus.NotRouted, cargo.Delivery.RoutingStatus);
         Assert.IsFalse(cargo.Delivery.IsMisdirected);
         Assert.IsNull(cargo.Delivery.EstimatedTimeOfArrival);
         //Assert.IsNull(cargo.Delivery.NextExpectedActivity);

         /* Use case 2: routing

            A number of possible routes for this cargo is requested and may be
            presented to the customer in some way for him/her to choose from.
            Selection could be affected by things like price and time of delivery,
            but this test simply uses an arbitrary selection to mimic that process.

            The cargo is then assigned to the selected route, described by an itinerary. */
         IList<Itinerary> itineraries = BookingService.RequestPossibleRoutesForCargo(trackingId);
         Itinerary itinerary = SelectPreferedItinerary(itineraries);
         cargo.AssignToRoute(itinerary);

         Assert.AreEqual(TransportStatus.NotReceived, cargo.Delivery.TransportStatus);
         Assert.AreEqual(RoutingStatus.Routed, cargo.Delivery.RoutingStatus);
         Assert.IsNotNull(cargo.Delivery.EstimatedTimeOfArrival);
         //Assert.AreEqual(new HandlingActivity(RECEIVE, HONGKONG), cargo.Delivery.nextExpectedActivity());

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
         HandlingEventService.RegisterHandlingEvent(
            new DateTime(2009, 3, 1), trackingId, SampleLocations.Hongkong.UnLocode, HandlingEventType.Receive
            );

         Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
         Assert.AreEqual(SampleLocations.Hongkong, cargo.Delivery.LastKnownLocation);

         // Next event: Load onto voyage CM003 in Hongkong
         HandlingEventService.RegisterHandlingEvent(
            new DateTime(2009, 3, 3), trackingId, SampleLocations.Hongkong.UnLocode, HandlingEventType.Load
            );

         // Check current state - should be ok
         //Assert.AreEqual(v100, cargo.Delivery.currentVoyage());
         Assert.AreEqual(SampleLocations.Hongkong, cargo.Delivery.LastKnownLocation);
         Assert.AreEqual(TransportStatus.OnboardCarrier, cargo.Delivery.TransportStatus);
         Assert.IsFalse(cargo.Delivery.IsMisdirected);
         //Assert.AreEqual(new HandlingActivity(UNLOAD, NEWYORK, v100), cargo.Delivery.nextExpectedActivity());


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


         // Cargo is now (incorrectly) unloaded in Tokyo
         HandlingEventService.RegisterHandlingEvent(
            new DateTime(2009, 3, 5), trackingId, SampleLocations.Tokyo.UnLocode, HandlingEventType.Unload
            );

         // Check current state - cargo is misdirected!
         //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
         Assert.AreEqual(SampleLocations.Tokyo, cargo.Delivery.LastKnownLocation);
         Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
         Assert.IsTrue(cargo.Delivery.IsMisdirected);
         //Assert.IsNull(cargo.Delivery.nextExpectedActivity());


         // -- Cargo needs to be rerouted --

         // TODO cleaner reroute from "earliest location from where the new route originates"

         // Specify a new route, this time from Tokyo (where it was incorrectly unloaded) to Stockholm
         RouteSpecification fromTokyo = new RouteSpecification(SampleLocations.Tokyo, SampleLocations.Stockholm, arrivalDeadline);
         cargo.SpecifyNewRoute(fromTokyo);

         // The old itinerary does not satisfy the new specification
         Assert.AreEqual(RoutingStatus.Misrouted, cargo.Delivery.RoutingStatus);
         //Assert.IsNull(cargo.Delivery.nextExpectedActivity());

         // Repeat procedure of selecting one out of a number of possible routes satisfying the route spec
         IList<Itinerary> newItineraries = BookingService.RequestPossibleRoutesForCargo(cargo.TrackingId);
         Itinerary newItinerary = SelectPreferedItinerary(newItineraries);
         cargo.AssignToRoute(newItinerary);

         // New itinerary should satisfy new route
         Assert.AreEqual(RoutingStatus.Routed, cargo.Delivery.RoutingStatus);

         // TODO we can't handle the face that after a reroute, the cargo isn't misdirected anymore
         //Assert.IsFalse(cargo.isMisdirected());
         //Assert.AreEqual(new HandlingActivity(LOAD, TOKYO), cargo.nextExpectedActivity());


         // -- Cargo has been rerouted, shipping continues --


         // Load in Tokyo
         HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 8), trackingId, SampleLocations.Tokyo.UnLocode, HandlingEventType.Load);

         // Check current state - should be ok
         //Assert.AreEqual(v300, cargo.Delivery.currentVoyage());
         Assert.AreEqual(SampleLocations.Tokyo, cargo.Delivery.LastKnownLocation);
         Assert.AreEqual(TransportStatus.OnboardCarrier, cargo.Delivery.TransportStatus);
         Assert.IsFalse(cargo.Delivery.IsMisdirected);
         //Assert.AreEqual(new HandlingActivity(UNLOAD, HAMBURG, v300), cargo.Delivery.nextExpectedActivity());

         // Unload in Hamburg
         //HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 12), trackingId, SampleLocations.Hamburg.UnLocode, HandlingEventType.Unload);

         //// Check current state - should be ok
         ////Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
         //Assert.AreEqual(SampleLocations.Hamburg, cargo.Delivery.LastKnownLocation);
         //Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
         //Assert.IsFalse(cargo.Delivery.IsMisdirected);
         ////Assert.AreEqual(new HandlingActivity(LOAD, HAMBURG, v400), cargo.Delivery.nextExpectedActivity());


         //// Load in Hamburg
         //HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 14), trackingId, SampleLocations.Hamburg.UnLocode, HandlingEventType.Load);

         //// Check current state - should be ok
         ////Assert.AreEqual(v400, cargo.Delivery.currentVoyage());
         //Assert.AreEqual(SampleLocations.Hamburg, cargo.Delivery.LastKnownLocation);
         //Assert.AreEqual(TransportStatus.OnboardCarrier, cargo.Delivery.TransportStatus);
         //Assert.IsFalse(cargo.Delivery.IsMisdirected);
         ////Assert.AreEqual(new HandlingActivity(UNLOAD, STOCKHOLM, v400), cargo.Delivery.nextExpectedActivity());


         // Unload in Stockholm
         HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 15), trackingId, SampleLocations.Stockholm.UnLocode, HandlingEventType.Unload);

         // Check current state - should be ok
         //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
         Assert.AreEqual(SampleLocations.Stockholm, cargo.Delivery.LastKnownLocation);
         Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
         Assert.IsFalse(cargo.Delivery.IsMisdirected);
         //Assert.AreEqual(new HandlingActivity(CLAIM, STOCKHOLM), cargo.Delivery.nextExpectedActivity());

         // Finally, cargo is claimed in Stockholm. This ends the cargo lifecycle from our perspective.
         HandlingEventService.RegisterHandlingEvent(new DateTime(2009, 3, 16), trackingId, SampleLocations.Stockholm.UnLocode, HandlingEventType.Claim);

         // Check current state - should be ok
         //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
         Assert.AreEqual(SampleLocations.Stockholm, cargo.Delivery.LastKnownLocation);
         Assert.AreEqual(TransportStatus.Claimed, cargo.Delivery.TransportStatus);
         Assert.IsFalse(cargo.Delivery.IsMisdirected);
         //Assert.IsNull(cargo.Delivery.nextExpectedActivity());
      }
      private Itinerary SelectPreferedItinerary(IList<Itinerary> itineraries)
      {
         return itineraries.First();
      }      
   }
}