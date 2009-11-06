using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Raised after cargo has been found to be misdirected.
   /// </summary>
   public class CargoWasMisdirectedEvent : DomainEvent<Cargo>
   {
      public CargoWasMisdirectedEvent(Cargo source) : base(source)
      {
      }
   }
}
