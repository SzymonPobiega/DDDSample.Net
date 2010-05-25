using DDDSample.Domain.Cargo;
using Reporting.Persistence.Raven;

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
         var cargo = _cargoDataAccess.FindByTrackingId(source.TrackingId.IdString);
         var spec = @event.NewSpecification;
         cargo.UpdateRouteSpecification(
            spec.Origin.CodeString,
            spec.Destination.CodeString,
            spec.ArrivalDeadline);

         _cargoDataAccess.Store(cargo);
      }
   }
}