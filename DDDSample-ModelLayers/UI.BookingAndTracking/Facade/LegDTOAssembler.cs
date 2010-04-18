using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Potential.Location;
using DDDSample.DomainModel.Potential.Voyage;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class LegDTOAssembler
   {
      private readonly ILocationRepository _locationRepository;
      private readonly IVoyageRepository _voyageRepository;

      public LegDTOAssembler(ILocationRepository locationRepository, IVoyageRepository voyageRepository)
      {
         _locationRepository = locationRepository;
         _voyageRepository = voyageRepository;
      }

      public Leg FromDTO(LegDTO legDto)
      {
         var voyage = _voyageRepository.Find(legDto.VoyageId);
         return new Leg(voyage, _locationRepository.Find(new UnLocode(legDto.From)),
            legDto.LoadTime,
            _locationRepository.Find(new UnLocode(legDto.To)),
            legDto.UnloadTime);
      }

      public LegDTO ToDTO(Leg leg)
      {
         return new LegDTO(leg.Voyage.Number.NumberString,
                           leg.Voyage.Id,
                           leg.LoadLocation.UnLocode.CodeString,
                           leg.UnloadLocation.UnLocode.CodeString,
                           leg.LoadDate,
                           leg.UnloadDate);
      }
   }
}