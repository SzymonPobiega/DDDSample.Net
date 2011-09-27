namespace DDDSample.Application.Commands
{
    /// <summary>
    /// Assigns cargo to a new route.
    /// </summary>
    public class AssignCargoToRouteCommand
    {
        public string TrackingId { get; set; }
        public RouteCandidateDTO Route { get; set; }
    }
}