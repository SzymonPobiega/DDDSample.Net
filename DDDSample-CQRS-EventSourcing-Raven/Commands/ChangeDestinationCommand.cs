using System;
using System.Linq;
using System.Collections.Generic;
using NServiceBus;

namespace DDDSample.Commands
{
   [Serializable]
   public class ChangeDestinationCommand
   {
      public string CargoId { get; set; }
      public string NewDestination { get; set; }
   }
}