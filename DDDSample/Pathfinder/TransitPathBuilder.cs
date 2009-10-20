using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// A helper class for builing transit paths.
   /// </summary>
   public class TransitPathBuilder
   {
      private readonly TransitPathBuilder _previous;
      private readonly RouteSpecification _specification;
      private readonly TransitEdge _edge;     

      public TransitPathBuilder(string origin, string destination, DateTime startTime)
      {         
         _specification = new RouteSpecification(origin, destination, startTime);         
      }

      private TransitPathBuilder(TransitPathBuilder previous, TransitEdge edge)
      {
         _previous = previous;
         _specification = previous._specification;
         _edge = edge;
      }

      public string LastLocation
      {
         get { return _edge != null ? _edge.To : _specification.Origin; }
      }

      public DateTime LastTime
      {
         get { return _edge != null ? _edge.ToDate : _specification.StartTime; }
      }

      public bool IsReady
      {
         get { return _edge != null && _edge.To == _specification.Destination; }
      }

      public TransitPathBuilder AddEdge(TransitEdge edge)
      {
         return new TransitPathBuilder(this, edge);
      }

      public TransitPath BuildPath()
      {
         List<TransitEdge> edges = new List<TransitEdge>();
         BuildPath(edges);
         return new TransitPath(edges);
      }

      private void BuildPath(ICollection<TransitEdge> edges)
      {
         if (_edge != null)
         {
            edges.Add(_edge);         
            _previous.BuildPath(edges);
         }
      }

      public bool CanAddEdge(TransitEdge edge)
      {
         if (_edge != null)
         {
            if (edge.To == _edge.To)
            {
               return false;
            }
            return _previous.CanAddEdge(edge);
         }
         return edge.To != _specification.Origin;
      }
   }
}