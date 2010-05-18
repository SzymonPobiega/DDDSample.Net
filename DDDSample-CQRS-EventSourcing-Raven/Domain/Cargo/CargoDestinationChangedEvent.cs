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
      public Delivery Delivery { get; private set; }
      public RouteSpecification NewSpecification { get; private set; }

      public CargoDestinationChangedEvent(RouteSpecification newSpecification, Delivery delivery)
      {
         Delivery = delivery;
         NewSpecification = newSpecification;
      }      
   }
}