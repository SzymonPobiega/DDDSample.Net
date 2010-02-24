using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// Represents data source for <see cref="GraphTraversalService"/>.
   /// </summary>
   public interface IGraphDAO
   {
      /// <summary>
      /// Gets a list of all edges of voyage graph.
      /// </summary>
      /// <returns></returns>
      IList<TransitEdge> GetRoutesFrom(string origin, DateTime notBefore);
   }
}