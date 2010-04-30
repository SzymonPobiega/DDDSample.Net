using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Handling;
using NServiceBus;

namespace DDDSample.Application.AsynchronousEventHandlers.Messages
{
   /// <summary>
   /// Message representing an event informing that cargo was handled.
   /// </summary>
   [Serializable]
   public class CargoWasHandledMessage : IMessage
   {
      public string TrackingId { get; set; }
      public HandlingEventType EventType { get; set; }
      public string Location { get; set; }
      public DateTime RegistrationDate { get; set; }
      public DateTime CompletionDate { get; set; }
   }
}
