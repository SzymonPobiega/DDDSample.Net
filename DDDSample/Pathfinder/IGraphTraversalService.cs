using System;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   public interface IGraphTraversalService
   {
      IList<TransitPath> FindPaths(String originUnLocode,
                                   String destinationUnLocode,
                                   IList<TransitEdge> graphEdges,
                                   Constraints limitations);
   }
}