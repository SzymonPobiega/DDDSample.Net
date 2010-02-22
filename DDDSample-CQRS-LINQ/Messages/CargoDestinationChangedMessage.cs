using System;
using System.Linq;
using System.Collections.Generic;
using NServiceBus;

namespace DDDSample.Messages
{
   [Serializable]
   public class CargoDestinationChangedMessage : IMessage
   {
      public string TrackingId { get; set; }
      public string Origin { get; set; }
      public string Destination { get; set; }
      public DateTime ArrivalDeadline { get; set; }
   }
}