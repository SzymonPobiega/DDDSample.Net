using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.DomainModel.Potential.Voyage;

namespace DDDSample.DomainModel.Potential.Voyage
{
   /// <summary>
   /// Provides access to voyage store.
   /// </summary>
   public interface IVoyageRepository
   {
      ///// <summary>
      ///// Finds voyage by its unique identifier (<see cref="VoyageNumber"/>).
      ///// </summary>
      ///// <param name="voyageNumber">Identifier.</param>
      ///// <returns>Requested voyage of null, if not found.</returns>
      //Voyage Find(VoyageNumber voyageNumber);
      IList<Voyage> FindBeginingBefore(DateTime deadline);
   }
}