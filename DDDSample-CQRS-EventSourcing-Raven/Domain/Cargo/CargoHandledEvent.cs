using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Signals that a cargo was handled.
   /// </summary>
   [Serializable]
   public class CargoHandledEvent : Event<Cargo>
   {
      public Delivery Delivery { get; private set; }

      public CargoHandledEvent()
      {
      }

      public CargoHandledEvent(Delivery delivery)
      {
         Delivery = delivery;
      }
   }
}
