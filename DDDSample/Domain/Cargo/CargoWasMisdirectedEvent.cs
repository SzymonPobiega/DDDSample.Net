using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.DomainModel.Operations.Cargo
{
   /// <summary>
   /// Raised after cargo has been found to be misdirected.
   /// </summary>
   public class CargoWasMisdirectedEvent : DomainEvent<DomainModel.Operations.Cargo.Cargo>
   {
      public CargoWasMisdirectedEvent(DomainModel.Operations.Cargo.Cargo source) : base(source)
      {
      }
   }
}