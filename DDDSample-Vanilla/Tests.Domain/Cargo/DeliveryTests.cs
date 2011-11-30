using System;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace DDDSample.Domain.Tests.Cargo
{
    [TestFixture]
    public class DeliveryTests
    {
        protected static readonly Location.Location Krakow = new Location.Location(new UnLocode("PLKRK"), "Krakow");
        protected static readonly Location.Location Warszawa = new Location.Location(new UnLocode("PLWAW"), "Warszawa");
        protected static readonly Location.Location Wroclaw = new Location.Location(new UnLocode("PLWRC"), "Wroclaw");

        public static DateTime ArrivalDeadline = new DateTime(2011, 12, 24);
        

        [Test]
        public void Cargo_is_not_considered_unloaded_at_destination_if_there_are_no_recorded_handling_events()
        {
            var delivery = DeliveryStateAfterHandling(null);

            Assert.IsFalse(delivery.IsUnloadedAtDestination);
        }

        [Test]
        public void Cargo_is_not_considered_unloaded_at_destination_after_handling_unload_event_but_not_at_destiation()
        {
            var delivery = DeliveryStateAfterHandling(new HandlingEvent(HandlingEventType.Unload, Warszawa, new DateTime(2012, 12, 10), new DateTime(2012, 12, 10), null));

            Assert.IsFalse(delivery.IsUnloadedAtDestination);
        }

        [Test]
        public void Cargo_is_not_considered_unloaded_at_destination_after_handling_other_event_at_destiation()
        {
            var delivery = DeliveryStateAfterHandling(new HandlingEvent(HandlingEventType.Customs, Wroclaw, new DateTime(2012, 12, 10), new DateTime(2012, 12, 10), null));

            Assert.IsFalse(delivery.IsUnloadedAtDestination);
        }

        [Test]
        public void Cargo_is_considered_unloaded_at_destination_after_handling_unload_event_at_destiation()
        {
            var delivery = DeliveryStateAfterHandling(new HandlingEvent(HandlingEventType.Unload, Wroclaw, new DateTime(2012, 12, 10), new DateTime(2012, 12, 10), null));

            Assert.IsTrue(delivery.IsUnloadedAtDestination);
        }

        private static Delivery DeliveryStateAfterHandling(HandlingEvent lastEvent)
        {
            return Delivery.DerivedFrom(RouteSpecification(), Itinerary(), lastEvent);
        }

        private static RouteSpecification RouteSpecification()
        {
            return new RouteSpecification(Krakow, Wroclaw, ArrivalDeadline);
        }

        private static Itinerary Itinerary()
        {
            return new Itinerary(new[]
                                 {
                                    new Leg(null, Krakow, new DateTime(2011, 12, 1), Warszawa, new DateTime(2011, 12, 10)),
                                    new Leg(null, Warszawa, new DateTime(2011, 12, 12), Wroclaw, new DateTime(2011, 12, 15))                                                   
                                 });
        }
    }
}
// ReSharper restore InconsistentNaming
