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
      private readonly IGraphDAO _dao;

      public GraphTraversalService(IGraphDAO dao)
      {
         _dao = dao;
      }

      /// <summary>
      /// Finds all paths which conply with provided route specification and are within
      /// provided constrtraints.
      /// </summary>
      /// <param name="originUnLocode"></param>
      /// <param name="destinationUnLocode"></param>
      /// <param name="limitations"></param>
      /// <returns></returns>
      public IList<TransitPath> FindPaths(String originUnLocode,
                                                    String destinationUnLocode,
                                                    Constraints limitations)
      {
         List<TransitPath> paths = new List<TransitPath>();
         BuildPaths(paths, new TransitPathBuilder(originUnLocode, destinationUnLocode, DateTime.Now));
         return paths.Where(x => x.Edges.Last().ToDate < limitations.ArrivalDeadline)
            .OrderBy(x => x.Edges.Last().ToDate).ToList();
      }

      private void BuildPaths(ICollection<TransitPath> paths, TransitPathBuilder currentPath)
      {
         foreach (TransitEdge edge in _dao.GetRoutesFrom(currentPath.LastLocation, currentPath.LastTime ))
         {
            if (currentPath.CanAddEdge(edge))
            {
               TransitPathBuilder newBuilder = currentPath.AddEdge(edge);
               if (newBuilder.IsReady)
               {
                  paths.Add(newBuilder.BuildPath());
               }
               else
               {
                  BuildPaths(paths, newBuilder);
               }
            }
         }
      }
   }


   //public IList<TransitPath> FindShortestPaths(String originUnLocode,
   //                                              String destinationUnLocode,
   //                                              Constraints limitations)
   //{
   //   DateTime currentDate = NextDate(DateTime.Now);

   //   IList<String> allVertices = dao.ListLocations();
   //   allVertices.Remove(originUnLocode);
   //   allVertices.Remove(destinationUnLocode);

   //   int candidateCount = GetRandomNumberOfCandidates();
   //   List<TransitPath> candidates = new List<TransitPath>();

   //   for (int i = 0; i < candidateCount; i++)
   //   {
   //      allVertices = GetRandomChunkOfLocations(allVertices);
   //      List<TransitEdge> transitEdges = new List<TransitEdge>();
   //      String firstLegTo = allVertices.First();

   //      DateTime fromDate = NextDate(currentDate);
   //      DateTime toDate = NextDate(fromDate);
   //      currentDate = NextDate(toDate);

   //      transitEdges.Add(new TransitEdge(dao.GetVoyageNumber(originUnLocode, firstLegTo),
   //        originUnLocode, firstLegTo, fromDate, toDate));

   //      for (int j = 0; j < allVertices.Count - 1; j++)
   //      {
   //         String curr = allVertices[j];
   //         String next = allVertices[j + 1];
   //         transitEdges.Add(GetNextEdge(ref currentDate, curr, next));
   //      }

   //      String lastLegFrom = allVertices.Last();
   //      transitEdges.Add(GetNextEdge(ref currentDate, lastLegFrom, destinationUnLocode));

   //      candidates.Add(new TransitPath(transitEdges));
   //   }

   //   return candidates;
   //}

   //private TransitEdge GetNextEdge(ref DateTime currentDate, string curr, string next)
   //{
   //   DateTime fromDate = NextDate(currentDate);
   //   DateTime toDate = NextDate(fromDate);
   //   currentDate = NextDate(toDate);
   //   return new TransitEdge(dao.GetVoyageNumber(curr, next), curr, next, fromDate, toDate);
   //}

   //private DateTime NextDate(DateTime currentDate)
   //{
   //   return currentDate.AddDays(1).AddMinutes(random.Next(1000) - 500);
   //}

   //private int GetRandomNumberOfCandidates()
   //{
   //   return 3 + random.Next(3);
   //}

   //private IList<String> GetRandomChunkOfLocations(IEnumerable<string> allLocations)
   //{
   //   return allLocations.Select(x => new { Value = x, Index = random.Next(100) })
   //      .OrderBy(x => x.Index)
   //      .Select(x => x.Value)
   //      .ToList();
   //}
}
