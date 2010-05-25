using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using NUnit.Framework;

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
         var itinerary = new Itinerary(new Leg[] { });
         var @event = new HandlingEvent(HandlingEventType.Claim, Krakow, DateTime.Now, DateTime.Now, null);

         itinerary.ShouldNotExpect(@event);
      }

      [Test]
      public void Receive_event_is_expected_when_first_leg_load_location_matches_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Receive, Krakow);

         itinerary.ShouldExpect(@event);
      }

      private static HandlingEvent Event(HandlingEventType eventType, Location.Location location)
      {
         return new HandlingEvent(eventType, location, DateTime.Now, DateTime.Now, null);
      }

      private static Itinerary FromKrakowToWroclaw()
      {
         return new Itinerary(new[]
                                 {
                                    new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                    new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                 });
      }

      [Test]
      public void Receive_event_is_not_expected_when_first_leg_load_location_doesnt_match_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Receive, Warszawa);

         itinerary.ShouldNotExpect(@event);
      }

      [Test]
      public void Claim_event_is_expected_when_last_leg_unload_location_matches_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Claim, Wroclaw);

         itinerary.ShouldExpect(@event);
      }

      [Test]
      public void Claim_event_is_not_expected_when_last_leg_unload_location_doesnt_match_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Claim, Krakow);

         itinerary.ShouldNotExpect(@event);
      }

      [Test]
      public void Load_event_is_expected_when_first_leg_load_location_matches_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Load, Krakow);

         itinerary.ShouldExpect(@event);
      }

      [Test]
      public void Load_event_is_expected_when_second_leg_load_location_matches_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Load, Warszawa);

         itinerary.ShouldExpect(@event);
      }

      [Test]
      public void Load_event_is_not_expected_when_event_location_doesnt_match_any_legs_load_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Load, Wroclaw);

         itinerary.ShouldNotExpect(@event);
      }

      [Test]
      public void Unload_event_is_expected_when_first_leg_unload_location_matches_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Unload, Warszawa);

         itinerary.ShouldExpect(@event);
      }

      [Test]
      public void Unload_event_is_expected_when_second_leg_unload_location_matches_event_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Unload, Wroclaw);

         itinerary.ShouldExpect(@event);
      }

      [Test]
      public void Load_event_is_not_expected_when_event_location_doesnt_match_any_legs_unload_location()
      {
         var itinerary = FromKrakowToWroclaw();
         var @event = Event(HandlingEventType.Unload, Krakow);

         itinerary.ShouldNotExpect(@event);
      }      
   }

   public static class ItineraryExtensions
   {
      public static void ShouldExpect(this Itinerary itinerary, HandlingEvent @event)
      {
         Assert.IsTrue(itinerary.IsExpected(@event));
      }
      public static void ShouldNotExpect(this Itinerary itinerary, HandlingEvent @event)
      {
         Assert.IsFalse(itinerary.IsExpected(@event));
      }
   }
  
}