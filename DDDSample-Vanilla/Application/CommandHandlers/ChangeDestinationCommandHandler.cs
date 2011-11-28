using System;
using DDDSample.Application.Commands;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Potential.Location;
using LeanCommandUnframework;

namespace DDDSample.Application.CommandHandlers
{
    public class ChangeDestinationCommandHandler : IHandler<ChangeDestinationCommand>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ICargoRepository _cargoRepository;

        public ChangeDestinationCommandHandler(ILocationRepository locationRepository, ICargoRepository cargoRepository)
        {
            _locationRepository = locationRepository;
            _cargoRepository = cargoRepository;
        }

        public object Handle(ChangeDestinationCommand command)
        {
            var trackingId = new TrackingId(command.TrackingId);
            var cargo = _cargoRepository.Find(trackingId);
            var routeSpecification = GetNewRouteSpecification(command, cargo);
            cargo.SpecifyNewRoute(routeSpecification);
            return null;
        }

        private RouteSpecification GetNewRouteSpecification(ChangeDestinationCommand command, Cargo cargo)
        {
            var destinationUnLocode = new UnLocode(command.NewDestination);
            var destination = _locationRepository.Find(destinationUnLocode);

            return new RouteSpecification(cargo.RouteSpecification.Origin, destination, cargo.RouteSpecification.ArrivalDeadline);
        }
    }
}