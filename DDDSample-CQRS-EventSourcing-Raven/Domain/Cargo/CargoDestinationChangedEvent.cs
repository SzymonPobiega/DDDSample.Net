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
   public class CargoDestinationChangedEvent : Event<Cargo>
   {
      private readonly Delivery _delivery;
      private readonly RouteSpecification _newSpecification;

      public CargoDestinationChangedEvent(RouteSpecification newSpecification, Delivery delivery)
      {
         _delivery = delivery;
         _newSpecification = newSpecification;
      }

      public Delivery Delivery
      {
         get { return _delivery; }
      }

      public RouteSpecification NewSpecification
      {
         get { return _newSpecification; }
      }
   }
}