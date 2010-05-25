using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NServiceBus;

namespace DDDSample.CommandHandlers
{
   public class RegisterHandlingEventCommandHandler : ICommandHandler<RegisterHandlingEventCommand>
   {
      private readonly ICargoRepository _cargoRepository;

      public RegisterHandlingEventCommandHandler(ICargoRepository cargoRepository)
      {
         _cargoRepository = cargoRepository;
      }

      public void Handle(RegisterHandlingEventCommand message)
      {
         Cargo cargo = _cargoRepository.Find(message.CargoId);

         cargo.RegisterHandlingEvent(message.Type, 
            new UnLocode(message.Location), 
            DateTime.Now, 
            message.CompletionTime);
      }
   }
}