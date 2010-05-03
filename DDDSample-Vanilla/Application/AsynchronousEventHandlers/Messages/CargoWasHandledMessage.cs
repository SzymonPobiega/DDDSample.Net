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
      public Guid EventUniqueId { get; set; }
   }
}
