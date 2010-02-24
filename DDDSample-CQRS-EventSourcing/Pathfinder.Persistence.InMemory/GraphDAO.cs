using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Pathfinder.Persistence.InMemory
{
   public class GraphDAO : IGraphDAO
   {
      private static readonly List<TransitEdge> _edges = new List<TransitEdge>();

      static GraphDAO()
      {
         
      }

      public IList<TransitEdge> GetRoutesFrom(string origin, DateTime notBefore)
      {
         return _edges.Where(x => x.From == origin && x.FromDate >= notBefore).ToList();
      }
   }
}
