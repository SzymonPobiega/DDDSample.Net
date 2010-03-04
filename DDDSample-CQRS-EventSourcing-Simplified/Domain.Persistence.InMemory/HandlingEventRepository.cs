using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;

namespace DDDSample.Domain.Persistence.InMemory
{
   public class HandlingEventRepository : IHandlingEventRepository
   {
      private static readonly Dictionary<TrackingId, HandlingHistory> _storage = new Dictionary<TrackingId, HandlingHistory>();

      public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId cargoTrackingId)
      {
         HandlingHistory existing;
         if (!_storage.TryGetValue(cargoTrackingId, out existing))
         {
            return null;
         }
         return existing;
      }

      public void Store(HandlingHistory handlingHistory)
      {
         _storage[handlingHistory.TrackingId] = handlingHistory;
      }
   }
}