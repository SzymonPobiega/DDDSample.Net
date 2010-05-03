using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace DDDSample.Domain.Handling
{
   /// <summary>
   /// Provides access to cargo handling history.
   /// </summary>
   public interface IHandlingEventRepository
   {      
      /// <summary>
      /// Returns handling history of cargo with provided tracking id.
      /// </summary>
      /// <param name="cargoTrackingId">Cargo tracking id.</param>
      /// <returns>Cargo handling history.</returns>
      HandlingHistory LookupHandlingHistoryOfCargo(TrackingId cargoTrackingId);

      /// <summary>
      /// Stores new handling event object.
      /// </summary>
      /// <param name="handlingEvent">Object representing a cargo handling enent.</param>
      void Store(HandlingEvent handlingEvent);

      /// <summary>
      /// Finds handling event by its unique id.
      /// </summary>      
      HandlingEvent Find(Guid uniqueId);
   }
}