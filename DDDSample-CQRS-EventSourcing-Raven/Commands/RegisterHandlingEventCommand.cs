using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using NServiceBus;

namespace DDDSample.Commands
{
   [Serializable]
   public class RegisterHandlingEventCommand
   {
      public string CargoId { get; set; }
      public DateTime CompletionTime { get; set; }
      public string Location { get; set; }
      public HandlingEventType Type { get; set; }
   }
}