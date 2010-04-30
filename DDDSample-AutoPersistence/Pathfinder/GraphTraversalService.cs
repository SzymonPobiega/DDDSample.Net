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
   public class GraphTraversalService
   {
      private readonly GraphDAO _dao;
      private readonly Random _random = new Random();      

      public GraphTraversalService(GraphDAO dao)
      {
         _dao = dao;
      }

      public IList<TransitPath> FindShortestPaths(String originUnLocode,
                                                    String destinationUnLocode,
                                                    Constraints limitations)
      {
         DateTime currentDate = NextDate(DateTime.Now);

         IList<String> allVertices = _dao.GetAllLocations();
         allVertices.Remove(originUnLocode);
         allVertices.Remove(destinationUnLocode);

         int candidateCount = GetRandomNumberOfCandidates();
         List<TransitPath> candidates = new List<TransitPath>();

         for (int i = 0; i < candidateCount; i++)
         {
            allVertices = GetRandomChunkOfLocations(allVertices);
            List<TransitEdge> transitEdges = new List<TransitEdge>();
            String firstLegTo = allVertices.First();

            DateTime fromDate = NextDate(currentDate);
            DateTime toDate = NextDate(fromDate);
            currentDate = NextDate(toDate);

            transitEdges.Add(new TransitEdge(originUnLocode, firstLegTo, fromDate, toDate));

            for (int j = 0; j < allVertices.Count - 1; j++)
            {
               String curr = allVertices[j];
               String next = allVertices[j + 1];
               transitEdges.Add(GetNextEdge(ref currentDate, curr, next));
            }

            String lastLegFrom = allVertices.Last();
            transitEdges.Add(GetNextEdge(ref currentDate, lastLegFrom, destinationUnLocode));

            candidates.Add(new TransitPath(transitEdges));
         }

         return candidates;
      }

      private TransitEdge GetNextEdge(ref DateTime currentDate, string curr, string next)
      {
         DateTime fromDate = NextDate(currentDate);
         DateTime toDate = NextDate(fromDate);
         currentDate = NextDate(toDate);
         return new TransitEdge(curr, next, fromDate, toDate);
      }

      private DateTime NextDate(DateTime currentDate)
      {
         return currentDate.AddDays(1).AddMinutes(_random.Next(1000) - 500);
      }

      private int GetRandomNumberOfCandidates()
      {
         return 3 + _random.Next(3);
      }

      private IList<String> GetRandomChunkOfLocations(IEnumerable<string> allLocations)
      {
         int total = allLocations.Count();
         int chunk = total > 4 ? 1 + _random.Next(5) : total;
         return allLocations.Select(x => new { Value = x, Index = _random.Next(100) })
            .OrderBy(x => x.Index)
            .Select(x => x.Value)
            .Take(chunk)
            .ToList();
      }
   }
}
