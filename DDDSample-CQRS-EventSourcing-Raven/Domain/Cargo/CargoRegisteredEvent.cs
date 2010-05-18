using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Signals that either new cargo instance was registered in the system or route specification
   /// was changed for an existing cargo.
   /// </summary>
   [Serializable]
   public class CargoRegisteredEvent : Event<Cargo>
   {
      public Delivery Delivery { get; private set; }
      public RouteSpecification RouteSpecification { get; private set; }
      public TrackingId TrackingId { get; private set; }

      public CargoRegisteredEvent()
      {         
      }

      public CargoRegisteredEvent(TrackingId trackingId, RouteSpecification routeSpecification, Delivery delivery)
      {
         RouteSpecification = routeSpecification;
         TrackingId = trackingId;
         Delivery = delivery;         
      }      
   }
}