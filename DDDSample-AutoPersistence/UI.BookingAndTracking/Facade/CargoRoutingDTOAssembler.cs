using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// DTO assembler for <see cref="CargoRoutingDTO"/>.
   /// </summary>
   public class CargoRoutingDTOAssembler
   {
      private readonly LegDTOAssembler _legDTOAssembler;

      public CargoRoutingDTOAssembler(LegDTOAssembler legDtoAssembler)
      {
         _legDTOAssembler = legDtoAssembler;
      }
      
      public CargoRoutingDTO ToDTO(Cargo cargo)
      {         
         return new CargoRoutingDTO(            
            cargo.TrackingId.IdString,
            cargo.RouteSpecification.Origin.UnLocode.CodeString,
            cargo.RouteSpecification.Destination.UnLocode.CodeString,
            cargo.RouteSpecification.ArrivalDeadline,
            cargo.Delivery.RoutingStatus == RoutingStatus.Misrouted,
            ToLegDTOs(cargo.Itinerary));
      }

      public IList<LegDTO> ToLegDTOs(Itinerary itinerary)
      {
         if (itinerary == null)
         {
            return new List<LegDTO>();
         }
         return itinerary.Legs.Select(x => _legDTOAssembler.ToDTO(x)).ToList();
      }
   }
}