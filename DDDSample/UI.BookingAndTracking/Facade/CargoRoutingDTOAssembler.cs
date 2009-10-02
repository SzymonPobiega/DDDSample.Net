using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace UI.BookingAndTracking.Facade
{
   /// <summary>
   /// DTO assembler for <see cref="CargoRoutingDTO"/>.
   /// </summary>
   public class CargoRoutingDTOAssembler
   {
      /// <summary>
      /// Assembles a DTO from <see cref="Cargo"/> domain object.
      /// </summary>
      /// <param name="cargo">Domain object.</param>
      /// <returns>DTO.</returns>
      public CargoRoutingDTO ToDTO(Cargo cargo)
      {
         return new CargoRoutingDTO(
            cargo.TrackingId.IdString,
            cargo.RouteSpecification.Origin.UnLocode.CodeString,
            cargo.RouteSpecification.Destination.UnLocode.CodeString,
            cargo.RouteSpecification.ArrivalDeadline);
      }
   }
}