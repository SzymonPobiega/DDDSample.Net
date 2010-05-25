using System;
using System.Linq;
using System.Collections.Generic;
using NServiceBus;

namespace DDDSample.Commands
{
   [Serializable]
   public class AssignCargoToRouteCommand
   {
      public string CargoId { get; set; }
      public List<LegDTO> Legs { get; set; }
   }
}