using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Raised after cargo has arrived assigned to route.
   /// </summary>
   public sealed class CargoWasAssignedToRouteEvent
   {
      private readonly Cargo _cargo;
      private readonly Delivery _delivery;
      private readonly Itinerary _oldItinerary;
      private readonly Itinerary _newItinerary;

      public CargoWasAssignedToRouteEvent(Cargo cargo, Itinerary oldItinerary, Itinerary newItinerary, Delivery delivery)
      {
         _cargo = cargo;
         _delivery = delivery;
         _newItinerary = newItinerary;
         _oldItinerary = oldItinerary;
      }


      public Delivery Delivery
      {
         get { return _delivery; }
      }

      public Itinerary NewItinerary
      {
         get { return _newItinerary; }
      }

      public Cargo Cargo
      {
         get { return _cargo; }
      }

      /// <summary>
      /// Gets the route before assigning cargo to a new one.
      /// </summary>
      public Itinerary OldItinerary
      {
         get { return _oldItinerary; }
      }
   }
}
