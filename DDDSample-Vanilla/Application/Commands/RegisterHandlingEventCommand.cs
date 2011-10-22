using System;
using DDDSample.DomainModel.Operations.Handling;

namespace DDDSample.Application.Commands
{
    /// <summary>
    /// Registers a handling event in the system, and notifies interested parties that a 
    /// cargo has been handled.
    /// </summary>
    public class RegisterHandlingEventCommand
    {
        public DateTime CompletionTime { get; set; }
        public string TrackingId { get; set; }
        public string OccuranceLocation { get; set; }
        public HandlingEventType Type { get; set; }
    }
}