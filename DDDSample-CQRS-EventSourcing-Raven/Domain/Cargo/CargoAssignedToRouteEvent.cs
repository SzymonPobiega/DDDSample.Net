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
      public Delivery Delivery { get; private set; }
      public Itinerary NewItinerary { get; private set; }

      public CargoAssignedToRouteEvent(Itinerary newItinerary, Delivery delivery)
      {
         Delivery = delivery;
         NewItinerary = newItinerary;
      }      
   }
}
