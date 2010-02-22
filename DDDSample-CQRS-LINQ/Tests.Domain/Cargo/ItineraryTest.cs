using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NUnit.Framework;

namespace DDDSample.Domain.Tests.Cargo
{
   [TestFixture]
   public class ItineraryTest
   {
      private static readonly Location.Location Krakow = new Location.Location(new UnLocode("PLKRK"), "Krakow");
      private static readonly Location.Location Warszawa = new Location.Location(new UnLocode("PLWAW"), "Warszawa");
      private static readonly Location.Location Wroclaw = new Location.Location(new UnLocode("PLWRC"), "Wroclaw");

      [Test]
      public void IsExpected_ClaimEvent_Empty_False()
      {
         Itinerary itinerary = new Itinerary(new Leg[] { });
         HandlingEvent @event = new HandlingEvent(HandlingEventType.Claim, Krakow, DateTime.Now, DateTime.Now, null);

         Assert.IsFalse(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_ReceiveEvent_FirstLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]{new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now  )});
         HandlingEvent @event = new HandlingEvent(HandlingEventType.Receive, Krakow, DateTime.Now, DateTime.Now, null);

         Assert.IsTrue(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_ReceiveEvent_FirstLegLocationDoesntMatchEventLocation_False()
      {
         Itinerary itinerary = new Itinerary(new[] { new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now) });
         HandlingEvent @event = new HandlingEvent(HandlingEventType.Receive, Warszawa, DateTime.Now, DateTime.Now, null);

         Assert.IsFalse(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_ClainEvent_LastLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });
         HandlingEvent @event = new HandlingEvent(HandlingEventType.Claim, Wroclaw, DateTime.Now, DateTime.Now, null);

         Assert.IsTrue(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_ClainEvent_LastLegLocationDoesntMatchEventLocation_False()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });
         HandlingEvent @event = new HandlingEvent(HandlingEventType.Claim, Warszawa, DateTime.Now, DateTime.Now, null);

         Assert.IsFalse(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_LoadEvent_FirstLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });

         HandlingEvent @event = new HandlingEvent(HandlingEventType.Load, Krakow, DateTime.Now, DateTime.Now, null);         

         Assert.IsTrue(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_LoadEvent_SecondLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });

         HandlingEvent @event = new HandlingEvent(HandlingEventType.Load, Warszawa, DateTime.Now, DateTime.Now, null);

         Assert.IsTrue(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_LoadEvent_NoLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });

         HandlingEvent @event = new HandlingEvent(HandlingEventType.Load, Wroclaw, DateTime.Now, DateTime.Now, null);

         Assert.IsFalse(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_UnloadEvent_FirstLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });

         HandlingEvent @event = new HandlingEvent(HandlingEventType.Unload, Warszawa, DateTime.Now, DateTime.Now, null);

         Assert.IsTrue(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_UnloadEvent_SecondLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });

         HandlingEvent @event = new HandlingEvent(HandlingEventType.Unload, Wroclaw, DateTime.Now, DateTime.Now, null);

         Assert.IsTrue(itinerary.IsExpected(@event));
      }

      [Test]
      public void IsExpected_UnloadEvent_NoLegLocationMathesEventLocation_True()
      {
         Itinerary itinerary = new Itinerary(new[]
                                                {
                                                   new Leg(Krakow, DateTime.Now, Warszawa, DateTime.Now),
                                                   new Leg(Warszawa, DateTime.Now, Wroclaw, DateTime.Now)                                                   
                                                });

         HandlingEvent @event = new HandlingEvent(HandlingEventType.Unload, Krakow, DateTime.Now, DateTime.Now, null);

         Assert.IsFalse(itinerary.IsExpected(@event));
      }      
   }
}