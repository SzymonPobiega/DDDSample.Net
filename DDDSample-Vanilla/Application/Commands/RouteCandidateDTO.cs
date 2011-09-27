using System.Collections.Generic;

namespace DDDSample.Application.Commands
{
   public class RouteCandidateDTO
   {
      public IList<LegDTO> Legs { get; set; }
   }
}