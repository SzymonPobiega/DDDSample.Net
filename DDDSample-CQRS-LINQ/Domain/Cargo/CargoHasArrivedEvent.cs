using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Raised after cargo has arrived at destination.
   /// </summary>
   public class CargoHasArrivedEvent : DomainEvent<Cargo>
   {
      public CargoHasArrivedEvent(Cargo source) : base(source)
      {
      }
   }
}
