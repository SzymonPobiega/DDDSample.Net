using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Messages;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class RouteCandidateDTO
   {
      public RouteCandidateDTO()
      {
         Legs = new List<LegDTO>();
      }
      public RouteCandidateDTO(IList<LegDTO> legs)
      {
         Legs = legs;
      }

      public IList<LegDTO> Legs { get; set; }
   }
}