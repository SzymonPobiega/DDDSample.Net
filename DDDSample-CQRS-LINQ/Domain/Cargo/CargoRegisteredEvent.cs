using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Signals that either new cargo instance was registered in the system or route specification
   /// was changed for an existing cargo.
   /// </summary>
   public class CargoRegisteredEvent
   {
      private readonly Cargo _cargo;
      private readonly Delivery _delivery;
      private readonly RouteSpecification _routeSpecification;

      public CargoRegisteredEvent(Cargo cargo, RouteSpecification routeSpecification, Delivery delivery)
      {
         _cargo = cargo;
         _routeSpecification = routeSpecification;
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
      
      public Cargo Cargo
      {
         get { return _cargo; }
      }
   }
}