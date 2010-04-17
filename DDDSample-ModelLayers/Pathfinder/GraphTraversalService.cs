using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// A simple implementation of graph traversal service.
   /// </summary>
   public class GraphTraversalService : IGraphTraversalService
   {
      public IList<TransitPath> FindPaths(String originUnLocode, String destinationUnLocode, IList<TransitEdge> graphEdges, Constraints limitations)
      {         
         DateTime currentDate = DateTime.Now.Date;
         var result = new List<TransitPath>();
         var initialEdges = FindPossibleEdges(originUnLocode, currentDate, graphEdges, limitations);
         foreach (var edge in initialEdges)
         {
            BuildRoute(new List<TransitEdge>{edge}, destinationUnLocode, graphEdges, limitations, result);
         }
         return result;
      }

      private static void BuildRoute(IList<TransitEdge> currentPath, string destinationNode, IEnumerable<TransitEdge> edges, Constraints constraints, ICollection<TransitPath> paths)
      {
         var currentEdge = currentPath.Last();
         if (currentEdge.To == destinationNode)
         {
            var path = new TransitPath(currentPath);
            paths.Add(path);
            return;
         }
         var possibleEdges = FindPossibleEdges(currentEdge.To, currentEdge.ToDate, edges, constraints);
         foreach (var possibleEdge in possibleEdges)
         {
            var newPath = new List<TransitEdge>(currentPath) {possibleEdge};
            BuildRoute(newPath, destinationNode,edges, constraints, paths);
         }
      }

      private static IEnumerable<TransitEdge> FindPossibleEdges(string currentNode, DateTime currentTime, IEnumerable<TransitEdge> edges, Constraints constraints)
      {
         return edges.Where(x => x.From == currentNode &&
                                 x.FromDate >= currentTime &&
                                 x.ToDate <= constraints.ArrivalDeadline);
      }      
   }
}
