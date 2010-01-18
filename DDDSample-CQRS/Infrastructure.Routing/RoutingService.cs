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
      private readonly ILocationRepository _locatinRepository;
      private readonly GraphTraversalService _graphTraversalService;      

      public RoutingService(ILocationRepository locatinRepository, GraphTraversalService graphTraversalService)
      {
         _locatinRepository = locatinRepository;
         _graphTraversalService = graphTraversalService;
      }

      public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
      {
         IList<TransitPath> paths = _graphTraversalService.FindShortestPaths(
            routeSpecification.Origin.UnLocode.CodeString,
            routeSpecification.Destination.UnLocode.CodeString,
            new Constraints(routeSpecification.ArrivalDeadline));

         return paths.Select(x => ToItinerary(x)).ToList();
      }      

      private Itinerary ToItinerary(TransitPath path)
      {
         return new Itinerary(path.Edges.Select(x => ToLeg(x)));
      }

      private Leg ToLeg(TransitEdge edge)
      {
         return new Leg(
            _locatinRepository.Find(new UnLocode(edge.From)), 
            edge.FromDate,
            _locatinRepository.Find(new UnLocode(edge.To)),
            edge.ToDate);
      }
   }
}
