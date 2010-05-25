using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Location
{
   /// <summary>
   /// Provides access to location store.
   /// </summary>
   public interface ILocationRepository
   {      
      /// <summary>
      /// Finds all locations.
      /// </summary>
      /// <returns>A collection of all defined locations.</returns>
      IList<Location> FindAll();
   }
}
