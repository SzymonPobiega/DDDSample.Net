using DDDSample.Application.Commands;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Potential.Customer;
using DDDSample.DomainModel.Potential.Location;
using LeanCommandUnframework;

namespace DDDSample.Application.CommandHandlers
{
    public class BookNewCargoCommandHandler : IHandler<BookNewCargoCommand>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly ICustomerRepository _customerRepository;

        public BookNewCargoCommandHandler(ILocationRepository locationRepository, ICargoRepository cargoRepository, ICustomerRepository customerRepository)
        {
            _locationRepository = locationRepository;
            _customerRepository = customerRepository;
            _cargoRepository = cargoRepository;
        }

        public object Handle(BookNewCargoCommand command)
        {
            var routeSpecification = GetRouteSpecification(command);
            var trackingId = _cargoRepository.NextTrackingId();
            var orderingCustomer = _customerRepository.Find(command.OrderingCustomerLogin);
            var cargo = new Cargo(trackingId, routeSpecification, orderingCustomer);

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