using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.DomainModel.Operations.Cargo
{
   /// <summary>
   /// Raised after cargo has arrived at destination.
   /// </summary>
   public class CargoHasArrivedEvent : DomainEvent<DomainModel.Operations.Cargo.Cargo>
   {
      public CargoHasArrivedEvent(DomainModel.Operations.Cargo.Cargo source) : base(source)
      {
      }
   }
}