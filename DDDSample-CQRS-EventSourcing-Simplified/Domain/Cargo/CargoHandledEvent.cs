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
      private readonly Delivery _delivery;

      public CargoHandledEvent(Delivery delivery)
      {
         _delivery = delivery;
      }

      public Delivery Delivery
      {
         get { return _delivery; }
      }      
   }
}
