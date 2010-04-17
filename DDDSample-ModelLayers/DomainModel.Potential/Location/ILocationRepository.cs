using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.DomainModel.Potential.Location;

namespace DDDSample.Domain.Location
{
   /// <summary>
   /// Provides access to location store.
   /// </summary>
   public interface ILocationRepository
   {
      /// <summary>
      /// Finds a location using given <see cref="UnLocode"/>.
      /// </summary>
      /// <param name="locode"></param>
      /// <returns>A location identified by a given <see cref="UnLocode"/></returns>
      DomainModel.Potential.Location.Location Find(UnLocode locode);
      /// <summary>
      /// Finds all locations.
      /// </summary>
      /// <returns>A collection of all defined locations.</returns>
      IList<DomainModel.Potential.Location.Location> FindAll();
   }
}
