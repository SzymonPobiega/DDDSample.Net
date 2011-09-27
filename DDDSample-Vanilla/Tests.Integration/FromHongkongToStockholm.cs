using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Application.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
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
            var origin = Hongkong;
            var destination = Stockholm;
            var arrivalDeadline = DateTime.Now.AddDays(60);

            /* Use case 1: booking

               A new cargo is booked, and the unique tracking id is assigned to the cargo. */
            var bookNewCargoCommand = new BookNewCargoCommand
                                          {
                                              Origin = origin.UnLocode.CodeString,
                                              Destination = destination.UnLocode.CodeString,
                                              ArrivalDeadline = arrivalDeadline
                                          };
            var bookNewCargoCommandResult = (BookNewCargoCommandResult)CommandPipeline.Process(bookNewCargoCommand);
            TrackingId trackingId = new TrackingId(bookNewCargoCommandResult.TrackingId);

            /* The tracking id can be used to lookup the cargo in the repository.

               Important: The cargo, and thus the domain model, is responsible for determining
               the status of the cargo, whether it is on the right track or not and so on.
               This is core domain logic.

               Tracking the cargo basically amounts to presenting information extracted from
               the cargo aggregate in a suitable way. */
            var cargo = CargoRepository.Find(trackingId);
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
            var routeCandidates = RequestPossibleRoutesForCargo(trackingId);

            var selectedRoute = SelectPreferedItinerary(routeCandidates);

            AssignCargoToRoute(trackingId, selectedRoute);

            Assert.AreEqual(TransportStatus.NotReceived, cargo.Delivery.TransportStatus);
            Assert.AreEqual(RoutingStatus.Routed, cargo.Delivery.RoutingStatus);
            Assert.IsNotNull(cargo.Delivery.EstimatedTimeOfArrival);
            Assert.AreEqual(new HandlingActivity(HandlingEventType.Receive, Hongkong), cargo.Delivery.NextExpectedActivity);

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
            RegisterHandlingEvent(
               new DateTime(2009, 3, 1), trackingId, Hongkong.UnLocode, HandlingEventType.Receive
               );

            Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
            Assert.AreEqual(Hongkong, cargo.Delivery.LastKnownLocation);

            // Next event: Load onto voyage CM003 in Hongkong
            RegisterHandlingEvent(
               new DateTime(2009, 3, 3), trackingId, Hongkong.UnLocode, HandlingEventType.Load
               );

            // Check current state - should be ok
            //Assert.AreEqual(v100, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Hongkong, cargo.Delivery.LastKnownLocation);
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
            RegisterHandlingEvent(
               new DateTime(2009, 3, 5), trackingId, Tokyo.UnLocode, HandlingEventType.Unload
               );

            // Check current state - cargo is misdirected!
            //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Tokyo, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
            Assert.IsTrue(cargo.Delivery.IsMisdirected);
            //Assert.IsNull(cargo.Delivery.nextExpectedActivity());


            // -- Cargo needs to be rerouted --

            // TODO cleaner reroute from "earliest location from where the new route originates"

            // Specify a new route, this time from Tokyo (where it was incorrectly unloaded) to Stockholm
            var fromTokyo = new RouteSpecification(Tokyo, Stockholm, arrivalDeadline);
            cargo.SpecifyNewRoute(fromTokyo);

            // The old itinerary does not satisfy the new specification
            Assert.AreEqual(RoutingStatus.Misrouted, cargo.Delivery.RoutingStatus);
            //Assert.IsNull(cargo.Delivery.nextExpectedActivity());

            // Repeat procedure of selecting one out of a number of possible routes satisfying the route spec
            var newRoutes = RequestPossibleRoutesForCargo(trackingId);
            var newSelectedRoute = SelectPreferedItinerary(newRoutes);

            AssignCargoToRoute(trackingId, newSelectedRoute);

            // New itinerary should satisfy new route
            Assert.AreEqual(RoutingStatus.Routed, cargo.Delivery.RoutingStatus);

            // TODO we can't handle the face that after a reroute, the cargo isn't misdirected anymore
            //Assert.IsFalse(cargo.isMisdirected());
            //Assert.AreEqual(new HandlingActivity(LOAD, TOKYO), cargo.nextExpectedActivity());


            // -- Cargo has been rerouted, shipping continues --


            // Load in Tokyo
            RegisterHandlingEvent(new DateTime(2009, 3, 8), trackingId, Tokyo.UnLocode, HandlingEventType.Load);

            // Check current state - should be ok
            //Assert.AreEqual(v300, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Tokyo, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.OnboardCarrier, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            //Assert.AreEqual(new HandlingActivity(UNLOAD, HAMBURG, v300), cargo.Delivery.nextExpectedActivity());

            // Unload in Hamburg
            RegisterHandlingEvent(new DateTime(2009, 3, 12), trackingId, Hamburg.UnLocode, HandlingEventType.Unload);

            // Check current state - should be ok
            //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Hamburg, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            //Assert.AreEqual(new HandlingActivity(LOAD, HAMBURG, v400), cargo.Delivery.nextExpectedActivity());


            // Load in Hamburg
            RegisterHandlingEvent(new DateTime(2009, 3, 14), trackingId, Hamburg.UnLocode, HandlingEventType.Load);

            // Check current state - should be ok
            //Assert.AreEqual(v400, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Hamburg, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.OnboardCarrier, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            //Assert.AreEqual(new HandlingActivity(UNLOAD, STOCKHOLM, v400), cargo.Delivery.nextExpectedActivity());


            // Unload in Stockholm
            RegisterHandlingEvent(new DateTime(2009, 3, 15), trackingId, Stockholm.UnLocode, HandlingEventType.Unload);

            // Check current state - should be ok
            //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Stockholm, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.InPort, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            //Assert.AreEqual(new HandlingActivity(CLAIM, STOCKHOLM), cargo.Delivery.nextExpectedActivity());

            // Finally, cargo is claimed in Stockholm. This ends the cargo lifecycle from our perspective.
            RegisterHandlingEvent(new DateTime(2009, 3, 16), trackingId, Stockholm.UnLocode, HandlingEventType.Claim);

            // Check current state - should be ok
            //Assert.AreEqual(NONE, cargo.Delivery.currentVoyage());
            Assert.AreEqual(Stockholm, cargo.Delivery.LastKnownLocation);
            Assert.AreEqual(TransportStatus.Claimed, cargo.Delivery.TransportStatus);
            Assert.IsFalse(cargo.Delivery.IsMisdirected);
            //Assert.IsNull(cargo.Delivery.nextExpectedActivity());
        }

        private static void AssignCargoToRoute(TrackingId trackingId, RouteCandidateDTO selectedRoute)
        {
            var assignToRouteCommand = new AssignCargoToRouteCommand()
                                           {
                                               Route = selectedRoute,
                                               TrackingId = trackingId.IdString
                                           };

            CommandPipeline.Process(assignToRouteCommand);
        }

        private static IEnumerable<RouteCandidateDTO> RequestPossibleRoutesForCargo(TrackingId trackingId)
        {
            var requestPossibleRoutesForCargoCommand 
                = new RequestPossibleRoutesForCargoCommand
                      {
                          TrackingId = trackingId.IdString
                      };

            var result = (RequestPossibleRoutesForCargoCommandResult)CommandPipeline.Process(requestPossibleRoutesForCargoCommand);
            return result.RouteCandidates;
        }

        private static void RegisterHandlingEvent(DateTime completionTime, TrackingId trackingId, UnLocode occuranceLocation, HandlingEventType handlingEventType)
        {
            var registerHandlingEventCommand = new RegisterHandlingEventCommand()
                                                   {
                                                       CompletionTime = completionTime,
                                                       OccuranceLocation = occuranceLocation.CodeString,
                                                       TrackingId = trackingId.IdString,
                                                       Type = handlingEventType
                                                   };
            CommandPipeline.Process(registerHandlingEventCommand);
        }

        private static RouteCandidateDTO SelectPreferedItinerary(IEnumerable<RouteCandidateDTO> itineraries)
        {
            return itineraries.First();
        }
    }
}