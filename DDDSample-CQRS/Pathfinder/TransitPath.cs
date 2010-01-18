using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// Represents a path in voyage graph.
   /// </summary>
   public sealed class TransitPath
   {
      private readonly ReadOnlyCollection<TransitEdge> _edges;

      /// <summary>
      /// Creates new path.
      /// </summary>
      /// <param name="edges">A collection of edges.</param>
      public TransitPath(IList<TransitEdge> edges)
      {
         _edges = new ReadOnlyCollection<TransitEdge>(edges);
      }

      /// <summary>
      /// Gets edges of the path.
      /// </summary>
      public ReadOnlyCollection<TransitEdge> Edges
      {
         get { return _edges; }
      }
   }
}