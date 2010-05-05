using System;
using System.Collections.Generic;
using System.Text;
using DDDSample.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NServiceBus;

namespace DDDSample.CommandHandlers
{
   public class BookNewCargoCommandHandler : IMessageHandler<BookNewCargoCommand>
   {
      private readonly ICargoRepository _cargoRepository;

      public BookNewCargoCommandHandler(ICargoRepository cargoRepository)
      {
         _cargoRepository = cargoRepository;
      }

      public void Handle(BookNewCargoCommand message)
      {
         RouteSpecification routeSpecification = new RouteSpecification(
            new UnLocode(message.Origin),
            new UnLocode(message.Destination),
            message.ArrivalDeadline);

         TrackingId trackingId = _cargoRepository.NextTrackingId();
         Cargo cargo = new Cargo(trackingId, routeSpecification);

         _cargoRepository.Store(cargo);
      }
   }
}