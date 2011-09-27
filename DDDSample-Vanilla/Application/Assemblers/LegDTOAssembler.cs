using DDDSample.Application.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;

namespace DDDSample.Application.Assemblers
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
            var loadLocation = _locationRepository.Find(new UnLocode(legDto.From));
            var unloadLocation = _locationRepository.Find(new UnLocode(legDto.To));
            return new Leg(loadLocation, legDto.LoadTime, unloadLocation, legDto.UnloadTime);
        }

        public LegDTO ToDTO(Leg leg)
        {
            return new LegDTO
                       {
                           From = leg.LoadLocation.UnLocode.CodeString,
                           To = leg.UnloadLocation.UnLocode.CodeString,
                           LoadTime = leg.LoadDate,
                           UnloadTime = leg.UnloadDate,
                           VoyageNumber = "XXX"
                       };
        }
    }
}