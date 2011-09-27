namespace DDDSample.Application.Commands
{
    /// <summary>
    /// Requests a list of itineraries describing possible routes for this cargo.
    /// </summary>
    public class RequestPossibleRoutesForCargoCommand
    {
        public string TrackingId { get; set; }
    }
}