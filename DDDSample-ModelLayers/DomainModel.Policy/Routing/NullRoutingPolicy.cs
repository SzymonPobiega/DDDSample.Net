using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.Policy.Routing
{
   [Serializable]
   public class NullRoutingPolicy : IRoutingPolicy
   {
      public IEnumerable<Itinerary> SelectOptimal(IEnumerable<Itinerary> candidates)
      {
         return candidates;
      }
   }
}