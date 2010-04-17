using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.Policy.Routing
{
   public interface IRoutingPolicy
   {
      IEnumerable<Itinerary> SelectOptimal(IEnumerable<Itinerary> candidates);
   }
}