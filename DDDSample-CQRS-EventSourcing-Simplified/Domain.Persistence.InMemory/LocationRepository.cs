using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Persistence.InMemory
{
   public class LocationRepository : ILocationRepository
   {
      private static readonly List<Location.Location> _storage = new List<Location.Location>
      {
            new Location.Location(new UnLocode("CNHKG"), "Hongkong"),
            new Location.Location(new UnLocode("AUMEL"), "Melbourne"),
            new Location.Location(new UnLocode("SESTO"), "Stockholm"),
            new Location.Location(new UnLocode("FIHEL"), "Helsinki"),
            new Location.Location(new UnLocode("USCHI"), "Chicago"),
            new Location.Location(new UnLocode("JNTKO"), "Tokyo"),
            new Location.Location(new UnLocode("DEHAM"), "Hamburg"),
            new Location.Location(new UnLocode("CNSHA"), "Shanghai"),
            new Location.Location(new UnLocode("NLRTM"), "Rotterdam"),
            new Location.Location(new UnLocode("SEGOT"), "Göteborg"),
            new Location.Location(new UnLocode("CNHGH"), "Hangzhou"),
            new Location.Location(new UnLocode("USNYC"), "New York"),
            new Location.Location(new UnLocode("USDAL"), "Dallas"),
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