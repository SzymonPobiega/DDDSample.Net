using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Location;

namespace DDDSample.Pathfinder
{
   public class GraphDAO
   {
      private readonly ILocationRepository _locationRepository;

      public GraphDAO(ILocationRepository locationRepository)
      {
         _locationRepository = locationRepository;
      }

      public IList<string> GetAllLocations()
      {
         return _locationRepository.FindAll().Select(x => x.UnLocode.CodeString).ToList();
      }
   }   
}