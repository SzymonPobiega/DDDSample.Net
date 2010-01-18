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
      private readonly ILocationRepository _locationRepository;

      public LegDTOAssembler(ILocationRepository locationRepository)
      {
         _locationRepository = locationRepository;
      }

      public Leg FromDTO(LegDTO legDto)
      {
         return new Leg(
            _locationRepository.Find(new UnLocode(legDto.LoadLocation)),
            legDto.LoadDate,
            _locationRepository.Find(new UnLocode(legDto.UnloadLocation)),
            legDto.UnloadDate);
      }

      public LegDTO ToDTO(Leg leg)
      {
         return new LegDTO
                   {
                      LoadLocation = leg.LoadLocation.UnLocode.CodeString,
                      UnloadLocation = leg.UnloadLocation.UnLocode.CodeString,
                      LoadDate = leg.LoadDate,
                      UnloadDate = leg.UnloadDate
                   };            
      }
   }
}