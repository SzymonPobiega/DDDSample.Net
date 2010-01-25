using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Signals that either new cargo instance was registered in the system or route specification
   /// was changed for an existing cargo.
   /// </summary>
   public class CargoDestinationChangedEvent
   {
      private readonly Cargo _cargo;
      private readonly Delivery _delivery;
      private readonly RouteSpecification _oldSpecification;
      private readonly RouteSpecification _newSpecification;

      public CargoDestinationChangedEvent(Cargo cargo, RouteSpecification oldSpecification, RouteSpecification newSpecification, Delivery delivery)
      {
         _cargo = cargo;
         _delivery = delivery;
         _newSpecification = newSpecification;
         _oldSpecification = oldSpecification;
      }

      public Delivery Delivery
      {
         get { return _delivery; }
      }

      public RouteSpecification NewSpecification
      {
         get { return _newSpecification; }
      }

      public RouteSpecification OldSpecification
      {
         get { return _oldSpecification; }
      }

      public Cargo Cargo
      {
         get { return _cargo; }
      }
   }
}