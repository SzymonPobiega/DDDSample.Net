using DDDSample.Application.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Voyage;

namespace DDDSample.Application.Assemblers
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
            var loadLocation = _locationRepository.Find(new UnLocode(legDto.From));
            var unloadLocation = _locationRepository.Find(new UnLocode(legDto.To));
            var voyage = _voyageRepository.Find(legDto.VoyageId);
            return new Leg(voyage, loadLocation, legDto.LoadTime, unloadLocation, legDto.UnloadTime);
        }

        public LegDTO ToDTO(Leg leg)
        {
            return new LegDTO
            {
                From = leg.LoadLocation.UnLocode.CodeString,
                To = leg.UnloadLocation.UnLocode.CodeString,
                LoadTime = leg.LoadDate,
                UnloadTime = leg.UnloadDate,
                VoyageId = leg.Voyage.Id,
                VoyageNumber = leg.Voyage.Number.NumberString
            };
        }
    }
}