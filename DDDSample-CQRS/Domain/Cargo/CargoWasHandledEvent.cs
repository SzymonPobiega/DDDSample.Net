using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Signals that a cargo was handled.
   /// </summary>
   public class CargoWasHandledEvent
   {
      private readonly Cargo _cargo;
      private readonly Delivery _delivery;
      private readonly HandlingEventType _eventType;

      public CargoWasHandledEvent(Cargo cargo, Delivery delivery, HandlingEventType eventType)
      {
         _cargo = cargo;
         _eventType = eventType;
         _delivery = delivery;
      }

      public Delivery Delivery
      {
         get { return _delivery; }
      }

      public Cargo Cargo
      {
         get { return _cargo; }
      }

      public HandlingEventType EventType
      {
         get { return _eventType; }
      }
   }
}
