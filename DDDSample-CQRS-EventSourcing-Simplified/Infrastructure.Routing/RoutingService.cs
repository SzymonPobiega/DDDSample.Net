using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Pathfinder;

namespace Infrastructure.Routing
{
   public class RoutingService : IRoutingService
   {
      private readonly GraphTraversalService _graphTravesrsalService;      

      public RoutingService(GraphTraversalService graphTraversalService)
      {
         _graphTravesrsalService = graphTraversalService;
      }

      public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
      {
         IList<TransitPath> paths = _graphTravesrsalService.FindShortestPaths(
            routeSpecification.Origin.CodeString,
            routeSpecification.Destination.CodeString,
            new Constraints(routeSpecification.ArrivalDeadline));

         return paths.Select(x => ToItinerary(x)).ToList();
      }      

      private static Itinerary ToItinerary(TransitPath path)
      {
         return new Itinerary(path.Edges.Select(x => ToLeg(x)));
      }

      private static Leg ToLeg(TransitEdge edge)
      {
         return new Leg(
            new UnLocode(edge.From), 
            edge.FromDate,
            new UnLocode(edge.To),
            edge.ToDate);
      }
   }
}
