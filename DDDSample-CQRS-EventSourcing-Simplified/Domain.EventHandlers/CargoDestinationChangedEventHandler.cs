using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Reporting.Persistence.NHibernate;
using NHibernate;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoDestinationChangedEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoDestinationChangedEventHandler : IEventHandler<Cargo.Cargo, CargoDestinationChangedEvent>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoDestinationChangedEventHandler(CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
      }
      
      public void Handle(Cargo.Cargo source, CargoDestinationChangedEvent @event)
      {
         var cargo = _cargoDataAccess.Find(source.Id);
         var spec = @event.NewSpecification;
         cargo.UpdateRouteSpecification(
            spec.Origin.CodeString,
            spec.Destination.CodeString,
            spec.ArrivalDeadline);
      }
   }
}