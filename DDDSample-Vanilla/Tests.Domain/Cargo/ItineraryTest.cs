using System;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace DDDSample.Domain.Tests.Cargo
{
    [TestFixture]
    public class ItineraryTest
    {
        protected static readonly Location.Location Krakow = new Location.Location(new UnLocode("PLKRK"), "Krakow");
        protected static readonly Location.Location Warszawa = new Location.Location(new UnLocode("PLWAW"), "Warszawa");
        protected static readonly Location.Location Wroclaw = new Location.Location(new UnLocode("PLWRC"), "Wroclaw");

        [Test]
        public void Claim_event_is_not_expected_by_an_empty_itinerary()
        {
            var cargoWithEmptyItinerary = new Itinerary(new Leg[] { });

            cargoWithEmptyItinerary
                .IsNotExpectedToBe(HandlingEventType.Claim).In(Krakow);
        }

        [Test]
        public void Receive_event_is_expected_when_first_leg_load_location_matches_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
                .IsExpectedToBe(HandlingEventType.Receive).In(Krakow);
        }

        [Test]
        public void Receive_event_is_not_expected_when_first_leg_load_location_doesnt_match_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
                .IsNotExpectedToBe(HandlingEventType.Receive).In(Warszawa);
        }

        [Test]
        public void Claim_event_is_expected_when_last_leg_unload_location_matches_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsExpectedToBe(HandlingEventType.Claim).In(Wroclaw);
        }

        [Test]
        public void Claim_event_is_not_expected_when_last_leg_unload_location_doesnt_match_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsNotExpectedToBe(HandlingEventType.Claim).In(Krakow);
        }

        [Test]
        public void Load_event_is_expected_when_first_leg_load_location_matches_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsExpectedToBe(HandlingEventType.Load).In(Krakow);
        }

        [Test]
        public void Load_event_is_expected_when_second_leg_load_location_matches_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsExpectedToBe(HandlingEventType.Load).In(Warszawa);
        }

        [Test]
        public void Load_event_is_not_expected_when_event_location_doesnt_match_any_legs_load_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsNotExpectedToBe(HandlingEventType.Load).In(Wroclaw);
        }

        [Test]
        public void Unload_event_is_expected_when_first_leg_unload_location_matches_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsExpectedToBe(HandlingEventType.Unload).In(Warszawa);
        }

        [Test]
        public void Unload_event_is_expected_when_second_leg_unload_location_matches_event_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsExpectedToBe(HandlingEventType.Unload).In(Wroclaw);
        }

        [Test]
        public void Load_event_is_not_expected_when_event_location_doesnt_match_any_legs_unload_location()
        {
            Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
               .IsNotExpectedToBe(HandlingEventType.Unload).In(Krakow);
        }

        
        private static Itinerary Cargo_from_Krakow_via_Warszawa_to_Wroclaw()
        {
            return new Itinerary(new[]
                                 {
                                    new Leg(null, Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                    new Leg(null, Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                 });
        }
    }

    public static class ItineraryTestExtensions
    {
        public static ItineraryTestHelper IsExpectedToBe(this Itinerary itinerary, HandlingEventType eventType)
        {
            return new ItineraryTestHelper(true, eventType, itinerary);
        }
        public static ItineraryTestHelper IsNotExpectedToBe(this Itinerary itinerary, HandlingEventType eventType)
        {
            return new ItineraryTestHelper(false, eventType, itinerary);
        }
    }

    public class ItineraryTestHelper
    {
        private readonly bool _expected;
        private readonly HandlingEventType _eventType;
        private readonly Itinerary _itinerary;

        public ItineraryTestHelper(bool expected, HandlingEventType eventType, Itinerary itinerary)
        {
            _expected = expected;
            _itinerary = itinerary;
            _eventType = eventType;
        }

        public void In(Location.Location location)
        {
            if (_expected)
            {
                Assert.IsTrue(_itinerary.IsExpected(Event(_eventType, location)));
            }
            else
            {
                Assert.IsFalse(_itinerary.IsExpected(Event(_eventType, location)));
            }
        }

        private static HandlingEvent Event(HandlingEventType eventType, Location.Location location)
        {
            return new HandlingEvent(eventType, location, DateTime.Now, DateTime.Now, null);
        }

    }

}
// ReSharper restore InconsistentNaming
