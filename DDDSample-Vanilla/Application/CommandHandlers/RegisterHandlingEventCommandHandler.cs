using System;
using DDDSample.Application.Commands;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.DomainModel;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Potential.Location;
using LeanCommandUnframework;
using HandlingEvent = DDDSample.DomainModel.Operations.Handling.HandlingEvent;

namespace DDDSample.Application.CommandHandlers
{
    public class RegisterHandlingEventCommandHandler : IHandler<RegisterHandlingEventCommand>
    {
        private readonly IHandlingEventRepository _handlingEventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly IEventPublisher _eventPublisher;

        public RegisterHandlingEventCommandHandler(IHandlingEventRepository handlingEventRepository, ILocationRepository locationRepository, ICargoRepository cargoRepository, IEventPublisher eventPublisher)
        {
            _handlingEventRepository = handlingEventRepository;
            _cargoRepository = cargoRepository;
            _eventPublisher = eventPublisher;
            _locationRepository = locationRepository;
        }

        public object Handle(RegisterHandlingEventCommand command)
        {
            var trackingId = new TrackingId(command.TrackingId);
            var cargo = _cargoRepository.Find(trackingId);
            var occuranceLocationUnLocode = new UnLocode(command.OccuranceLocation);
            var occuranceLocation = _locationRepository.Find(occuranceLocationUnLocode);
            var evnt = new HandlingEvent(command.Type, occuranceLocation, DateTime.Now, command.CompletionTime, cargo, _eventPublisher);
            _handlingEventRepository.Store(evnt);

            return null;
        }
    }
}