using System.Linq;
using DDDSample.Application.Commands;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.Application.Assemblers
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
         return new Itinerary(routeCandidate.Legs.Select(FromLegDTO));
      }

       private Leg FromLegDTO(LegDTO x)
       {
           return _legDTOAssembler.FromDTO(x);
       }

       public RouteCandidateDTO ToDTO(Itinerary itinerary)
      {
          var legs = itinerary.Legs.Select(ToLegDTO).ToList();
          return new RouteCandidateDTO
                     {
                         Legs = legs
                     };
      }

       private LegDTO ToLegDTO(Leg x)
       {
           return _legDTOAssembler.ToDTO(x);
       }
   }
}