using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Pathfinder;
using DDDSample.Messages;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class RoutingFacade
   {     
      private readonly GraphTraversalService _graphTravesrsalService;

      public RoutingFacade(GraphTraversalService graphTraversalService)
      {
         _graphTravesrsalService = graphTraversalService;
      }

      /// <summary>
      /// Fetches all possible routes for delivering cargo with provided tracking id.
      /// </summary>
      /// <returns>Possible delivery routes</returns>
      public IList<RouteCandidateDTO> FetchRoutesForSpecification(string origin, string destination, DateTime arrivalDeadline)
      {
         IList<TransitPath> paths = _graphTravesrsalService.FindShortestPaths(
            origin,
            destination,
            new Constraints(arrivalDeadline));

         return paths.Select(x => ToRouteCandidate(x)).ToList();
      }      

      private static RouteCandidateDTO ToRouteCandidate(TransitPath path)
      {
         return new RouteCandidateDTO {Legs = path.Edges.Select(x => ToLeg(x)).ToList()};
      }

      private static LegDTO ToLeg(TransitEdge edge)
      {
         return new LegDTO
                   {
                      LoadDate = edge.FromDate,
                      LoadLocation = edge.From,
                      UnloadDate = edge.ToDate,
                      UnloadLocation = edge.To
                   };            
      }
   }
}