using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Raised after cargo has arrived assigned to route.
   /// </summary>
   [Serializable]
   public class CargoAssignedToRouteEvent : Event<Cargo>
   {
      private readonly Delivery _delivery;
      private readonly Itinerary _newItinerary;

      public CargoAssignedToRouteEvent(Itinerary newItinerary, Delivery delivery)
      {
         _delivery = delivery;
         _newItinerary = newItinerary;
      }


      public Delivery Delivery
      {
         get { return _delivery; }
      }

      public Itinerary NewItinerary
      {
         get { return _newItinerary; }
      }
   }
}
