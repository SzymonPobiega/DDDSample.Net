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
      /// <summary>
      /// Finds a collection of voyages witch begin before specified date.
      /// </summary>
      /// <param name="deadline">Deadline</param>
      /// <returns></returns>
      IList<Voyage> FindBeginingBefore(DateTime deadline);
      /// <summary>
      /// Finds a voyage by its unique id.
      /// </summary>
      /// <param name="voyageId"></param>
      /// <returns></returns>
      Voyage Find(Guid voyageId);
   }
}