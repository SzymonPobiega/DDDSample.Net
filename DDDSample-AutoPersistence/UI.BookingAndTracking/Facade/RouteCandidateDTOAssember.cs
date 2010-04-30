using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class RouteCandidateDTOAssember
   {
      private readonly LegDTOAssembler _legDTOAssembler;

      public RouteCandidateDTOAssember(LegDTOAssembler legDtoAssembler)
      {
         _legDTOAssembler = legDtoAssembler;
      }

      public Itinerary FromDTO(RouteCandidateDTO routeCandidate)
      {
         return new Itinerary(routeCandidate.Legs.Select(x => _legDTOAssembler.FromDTO(x)));
      }

      public RouteCandidateDTO ToDTO(Itinerary itinerary)
      {
         return new RouteCandidateDTO(itinerary.Legs.Select(x => _legDTOAssembler.ToDTO(x)).ToList());
      }      
   }
}