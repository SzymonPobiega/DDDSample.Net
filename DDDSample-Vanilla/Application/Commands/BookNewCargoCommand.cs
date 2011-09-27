using System;

namespace DDDSample.Application.Commands
{
    /// <summary>
    /// Registers a new cargo in the tracking system, not yet routed.
    /// </summary>
    public class BookNewCargoCommand
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ArrivalDeadline { get; set; }
    }
}