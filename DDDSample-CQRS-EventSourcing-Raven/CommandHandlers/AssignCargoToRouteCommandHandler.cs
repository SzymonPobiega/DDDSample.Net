using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NServiceBus;

namespace DDDSample.CommandHandlers
{
   public class AssignCargoToRouteCommandHandler : ICommandHandler<AssignCargoToRouteCommand>
   {
      private readonly ICargoRepository _cargoRepository;

      public AssignCargoToRouteCommandHandler(ICargoRepository cargoRepository)
      {
         _cargoRepository = cargoRepository;
      }

      public void Handle(AssignCargoToRouteCommand message)
      {
         Cargo cargo = _cargoRepository.Find(message.CargoId);
         var itinerary = new Itinerary(message.Legs.Select(x => new Leg(
                                                                         new UnLocode(x.LoadLocation),
                                                                         x.LoadDate,
                                                                         new UnLocode(x.UnloadLocation),
                                                                         x.UnloadDate)));
         
         cargo.AssignToRoute(itinerary);
      }
   }
}