using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Reporting;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class RouteCandidateDTO
   {
      public RouteCandidateDTO()
      {
         Legs = new List<Leg>();
      }
      public RouteCandidateDTO(IList<Leg> legs)
      {
         Legs = legs;
      }

      public IList<Leg> Legs { get; set; }
   }
}