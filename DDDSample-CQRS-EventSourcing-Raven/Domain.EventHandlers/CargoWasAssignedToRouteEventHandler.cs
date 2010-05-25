using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using Reporting.Persistence.Raven;
using Leg = DDDSample.Reporting.Leg;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoAssignedToRouteEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoWasAssignedToRouteEventHandler : IEventHandler<Cargo.Cargo, CargoAssignedToRouteEvent>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoWasAssignedToRouteEventHandler(CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      public void Handle(Cargo.Cargo source, CargoAssignedToRouteEvent @event)
      {
         var cargo = _cargoDataAccess.FindByTrackingId(source.TrackingId.IdString);
         cargo.RouteSpecification = ConvertItineraryToLegDtos(@event.NewItinerary);

         _cargoDataAccess.Store(cargo);
      }

      private static List<Leg> ConvertItineraryToLegDtos(Itinerary itinerary)
      {
         return itinerary.Legs.Select(x => ConvertLegToDto(x)).ToList();
      }

      private static Leg ConvertLegToDto(Cargo.Leg x)
      {
         return new Leg
         {
            LoadDate = x.LoadDate,
            LoadLocation = x.LoadLocation.CodeString,
            UnloadDate = x.UnloadDate,
            UnloadLocation = x.UnloadLocation.CodeString
         };
      }
   }
}