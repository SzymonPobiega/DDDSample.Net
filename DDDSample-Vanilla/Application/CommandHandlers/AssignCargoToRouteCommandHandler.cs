using System;
using DDDSample.Application.Assemblers;
using DDDSample.Application.Commands;
using DDDSample.Domain.Cargo;
using LeanCommandUnframework;

namespace DDDSample.Application.CommandHandlers
{
    public class AssignCargoToRouteCommandHandler : IHandler<AssignCargoToRouteCommand>
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly RouteCandidateDTOAssember _routeCandidateDTOAssember;

        public AssignCargoToRouteCommandHandler(ICargoRepository cargoRepository, RouteCandidateDTOAssember routeCandidateDTOAssember)
        {
            _cargoRepository = cargoRepository;
            _routeCandidateDTOAssember = routeCandidateDTOAssember;
        }

        public object Handle(AssignCargoToRouteCommand command)
        {
            var trackingId = new TrackingId(command.TrackingId);
            var cargo = _cargoRepository.Find(trackingId);

            var itinerary = _routeCandidateDTOAssember.FromDTO(command.Route);

            cargo.AssignToRoute(itinerary);
            return null;
        }
    }
}