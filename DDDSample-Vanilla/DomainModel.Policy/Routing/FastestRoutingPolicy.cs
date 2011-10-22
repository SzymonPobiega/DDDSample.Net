using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.Policy.Routing
{
   [Serializable]
   public class FastestRoutingPolicy : IRoutingPolicy
   {
      public IEnumerable<Itinerary> SelectOptimal(IEnumerable<Itinerary> candidates)
      {
         yield return candidates.OrderBy(x => x.FinalArrivalDate).First();
      }
   }
}