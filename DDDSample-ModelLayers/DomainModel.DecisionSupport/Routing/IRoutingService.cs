using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.DecisionSupport.Routing
{
   /// <summary>
   /// Definition of a routing external service.
   /// </summary>
   public interface IRoutingService
   {
      /// <summary>
      /// Finds all possible routes that satisfy a given specification.
      /// </summary>
      /// <param name="cargoToBeRouted">Cargo to be routed.</param>
      /// <returns>A list of itineraries that satisfy cargo's route specification. May be an empty list if no route is found.</returns>
      IList<Itinerary> FetchRoutesFor(Cargo cargoToBeRouted);
   }
}