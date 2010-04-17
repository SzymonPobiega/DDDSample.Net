using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace DDDSample.Application.AsynchronousEventHandlers.Messages
{
    /// <summary>
    /// Message representing an event informing that cargo has been assigned to a new route.
    /// </summary>
    [Serializable]
    public class CargoHasBeenAssignedToRouteMessage : IMessage
    {
        public string TrackingId { get; set; }
    }
}
