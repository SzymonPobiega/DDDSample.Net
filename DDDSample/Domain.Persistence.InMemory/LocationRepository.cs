using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Persistence.InMemory
{
   public class LocationRepository : ILocationRepository
   {
      private readonly List<Location.Location> _storage = new List<Location.Location>
                                                             {
                                                                new Location.Location(new UnLocode("PLKRK"),"Krakow" ),
                                                                new Location.Location(new UnLocode("PLWAW"),"Warszawa" ),
                                                                new Location.Location(new UnLocode("PLWRC"),"Wroclaw" )
                                                             };

      public Location.Location Find(UnLocode locode)
      {
         return _storage.FirstOrDefault(x => x.UnLocode == locode);
      }

      public IList<Location.Location> FindAll()
      {
         return _storage.AsReadOnly();
      }
   }
}