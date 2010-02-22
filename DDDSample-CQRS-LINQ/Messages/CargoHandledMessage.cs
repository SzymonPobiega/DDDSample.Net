using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace DDDSample.Messages
{
   /// <summary>
   /// Message representing an event informing that cargo was handled.
   /// </summary>
   [Serializable]
   public class CargoHandledMessage : IMessage
   {
      public string TrackingId { get; set; }
      public DateTime CalculatedAt { get; set; }

      public int? NextExpectedEventType { get; set; }
      public string NextExpectedLocation { get; set; }

      public int LastKnownEventType { get; set; }
      public string LastKnownLocation { get; set; }

      public int RoutingStatus { get; set; }
      public int TransportStatus { get; set; }

      public DateTime? EstimatedTimeOfArrival { get; set; }

      public bool IsUnloaded { get; set; }
      public bool IsMisdirected { get; set; }      
   }
}