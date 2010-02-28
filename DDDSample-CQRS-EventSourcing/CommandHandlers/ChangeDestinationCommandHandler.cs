using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NServiceBus;

namespace DDDSample.CommandHandlers
{
   public class ChangeDestinationCommandHandler : IMessageHandler<ChangeDestinationCommand>
   {
      private readonly ICargoRepository _cargoRepository;

      public ChangeDestinationCommandHandler(ICargoRepository cargoRepository)
      {
         _cargoRepository = cargoRepository;
      }

      public void Handle(ChangeDestinationCommand message)
      {
         Cargo cargo = _cargoRepository.Find(message.CargoId);
         cargo.SpecifyNewRoute(new UnLocode(message.NewDestination));
      }
   }
}