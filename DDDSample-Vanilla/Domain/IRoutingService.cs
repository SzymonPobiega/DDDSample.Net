using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;

namespace DDDSample.Domain
{
   /// <summary>
   /// Definition of a routing external service.
   /// </summary>
   public interface IRoutingService
   {
      /// <summary>
      /// Finds all possible routes that satisfy a given specification.
      /// </summary>
      /// <param name="routeSpecification">Description of route.</param>
      /// <returns>A list of itineraries that satisfy the specification. May be an empty list if no route is found.</returns>
      IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification);
   }
}
