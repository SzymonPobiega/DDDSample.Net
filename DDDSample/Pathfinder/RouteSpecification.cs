using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// Specification of a route to be found.
   /// </summary>
   public class RouteSpecification
   {
      private readonly DateTime _startTime;
      private readonly string _destination;
      private readonly string _origin;

      public RouteSpecification(string destination, string origin, DateTime startTime)
      {
         _destination = destination;
         _startTime = startTime;
         _origin = origin;
      }

      public DateTime StartTime
      {
         get { return _startTime; }
      }

      public string Origin
      {
         get { return _origin; }
      }

      public string Destination
      {
         get { return _destination; }
      }
   }
}