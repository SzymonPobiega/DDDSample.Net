using System.Collections.Generic;

namespace DDDSample.Application.Commands
{
    public class RequestPossibleRoutesForCargoCommandResult
    {
        public IList<RouteCandidateDTO> RouteCandidates { get; set; }
    }
}