using System;
using System.Collections.Generic;
using System.Linq;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.Policy.Routing
{
   [Serializable]
   public class CheapestRoutingPolicy : IRoutingPolicy
   {
      public IEnumerable<Itinerary> SelectOptimal(IEnumerable<Itinerary> candidates)
      {
         var candidatesWithCost = candidates.Select(x => new {Value = x, Cost = x.CalculateTotalCost()});
         var candidatesInOrder = candidatesWithCost.OrderBy(x => x.Cost);

         yield return candidatesInOrder.First().Value;
      }
   }
}