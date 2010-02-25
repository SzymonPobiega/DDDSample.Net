using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Messages;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class LegDTOAssembler
   {
      public Leg FromDTO(LegDTO legDto)
      {
         return new Leg(
            new UnLocode(legDto.LoadLocation),
            legDto.LoadDate,
            new UnLocode(legDto.UnloadLocation),
            legDto.UnloadDate);
      }

      public LegDTO ToDTO(Leg leg)
      {
         return new LegDTO
                   {
                      LoadLocation = leg.LoadLocation.CodeString,
                      UnloadLocation = leg.UnloadLocation.CodeString,
                      LoadDate = leg.LoadDate,
                      UnloadDate = leg.UnloadDate
                   };            
      }
   }
}