using System;
using DDDSample.Application.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using LeanCommandUnframework;

namespace DDDSample.Application.CommandHandlers
{
    public class BookNewCargoCommandHandler : IHandler<BookNewCargoCommand>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ICargoRepository _cargoRepository;

        public BookNewCargoCommandHandler(ILocationRepository locationRepository, ICargoRepository cargoRepository)
        {
            _locationRepository = locationRepository;
            _cargoRepository = cargoRepository;
        }

        public object Handle(BookNewCargoCommand command)
        {
            var routeSpecification = GetRouteSpecification(command);
            var trackingId = _cargoRepository.NextTrackingId();
            var cargo = new Cargo(trackingId, routeSpecification);

            _cargoRepository.Store(cargo);

            return new BookNewCargoCommandResult
                       {
                           TrackingId = trackingId.IdString
                       };
        }

        private RouteSpecification GetRouteSpecification(BookNewCargoCommand command)
        {
            var originUnLocode = new UnLocode(command.Origin);
            var origin = _locationRepository.Find(originUnLocode);
            var destinationUnLocode = new UnLocode(command.Destination);
            var destination = _locationRepository.Find(destinationUnLocode);

            return new RouteSpecification(origin, destination, command.ArrivalDeadline);
        }
    }
}