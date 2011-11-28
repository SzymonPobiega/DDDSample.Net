using System.Linq;
using DDDSample.Application.Assemblers;
using DDDSample.Application.Commands;
using DDDSample.DomainModel.DecisionSupport.Routing;
using DDDSample.DomainModel.Operations.Cargo;
using LeanCommandUnframework;

namespace DDDSample.Application.CommandHandlers
{
    public class RequestPossibleRoutesForCargoCommandHandler : IHandler<RequestPossibleRoutesForCargoCommand>
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly IRoutingService _routingService;
        private readonly RouteCandidateDTOAssember _routeCandidateDTOAssember;

        public RequestPossibleRoutesForCargoCommandHandler(ICargoRepository cargoRepository, IRoutingService routingService, RouteCandidateDTOAssember routeCandidateDTOAssember)
        {
            _cargoRepository = cargoRepository;
            _routeCandidateDTOAssember = routeCandidateDTOAssember;
            _routingService = routingService;
        }

        public object Handle(RequestPossibleRoutesForCargoCommand command)
        {
            var trackingId = new TrackingId(command.TrackingId);
            var cargo = _cargoRepository.Find(trackingId);
            var itineraries = _routingService.FetchRoutesFor(cargo);
            var routeCandidates = itineraries.Select(x => _routeCandidateDTOAssember.ToDTO(x));

            return new RequestPossibleRoutesForCargoCommandResult
                       {
                           RouteCandidates = routeCandidates.ToList()
                       };
        }
    }
}