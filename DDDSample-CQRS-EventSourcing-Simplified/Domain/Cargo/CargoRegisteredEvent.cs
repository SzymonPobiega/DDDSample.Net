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
      private readonly TrackingId _trackingId;
      private readonly Delivery _delivery;
      private readonly RouteSpecification _routeSpecification;

      public CargoRegisteredEvent(TrackingId trackingId, RouteSpecification routeSpecification, Delivery delivery)
      {
         _routeSpecification = routeSpecification;
         _trackingId = trackingId;
         _delivery = delivery;         
      }

      public Delivery Delivery
      {
         get { return _delivery; }
      }

      public RouteSpecification RouteSpecification
      {
         get { return _routeSpecification; }
      }

      public TrackingId TrackingId
      {
         get { return _trackingId; }
      }
   }
}