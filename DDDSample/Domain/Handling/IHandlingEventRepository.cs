using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.Operations.Handling
{
   /// <summary>
   /// Provides access to cargo handling history.
   /// </summary>
   public interface IHandlingEventRepository
   {
      /*
       * TODO: Przemyœleæ dlaczego tu jest TrackingId, a w HandlingEvent Cargo.
       */
      /// <summary>
      /// Returns handling history of cargo with provided tracking id.
      /// </summary>
      /// <param name="cargoTrackingId">Cargo tracking id.</param>
      /// <returns>Cargo handling history.</returns>
      HandlingHistory LookupHandlingHistoryOfCargo(TrackingId cargoTrackingId);

      /// <summary>
      /// Stores new history object.
      /// </summary>
      /// <param name="handlingHistory">Object representing cargo handling history.</param>
      void Store(HandlingHistory handlingHistory);
   }
}