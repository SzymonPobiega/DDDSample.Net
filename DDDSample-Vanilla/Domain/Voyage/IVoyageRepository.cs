using System;
using System.Collections.Generic;

namespace DDDSample.Domain.Voyage
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