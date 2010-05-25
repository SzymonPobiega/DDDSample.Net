using DDDSample.Domain.Cargo;
using Reporting.Persistence.Raven;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoRegisteredEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoRegisteredEventHandler : IEventHandler<Cargo.Cargo, CargoRegisteredEvent>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoRegisteredEventHandler(CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      public void Handle(Cargo.Cargo source, CargoRegisteredEvent @event)
      {
         var spec = @event.RouteSpecification;
         var cargo = new Reporting.Cargo(source.Id,
                                         @event.TrackingId.IdString,
                                         spec.Origin.CodeString,
                                         spec.Destination.CodeString,
                                         spec.ArrivalDeadline);

         _cargoDataAccess.Store(cargo);
      }
   }
}