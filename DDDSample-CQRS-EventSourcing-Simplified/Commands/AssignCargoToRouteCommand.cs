using System;
using System.Linq;
using System.Collections.Generic;
using NServiceBus;

namespace DDDSample.Commands
{
   [Serializable]
   public class AssignCargoToRouteCommand : IMessage
   {
      public Guid CargoId { get; set; }
      public List<LegDTO> Legs { get; set; }
   }
}