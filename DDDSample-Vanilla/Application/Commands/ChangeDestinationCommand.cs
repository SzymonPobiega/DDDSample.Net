namespace DDDSample.Application.Commands
{
    /// <summary>
    /// Changes the destination of a cargo.
    /// </summary>
    public class ChangeDestinationCommand
    {
        public string TrackingId { get; set; }
        public string NewDestination { get; set; }
    }
}