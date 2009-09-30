using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   public class RouteSpecification : ValueObject
   {
      private readonly Location.Location _origin;
      private readonly Location.Location _destination;
      private readonly DateTime _arrivalDeadline;

      public RouteSpecification(Location.Location origin, Location.Location destination, DateTime arrivalDeadline)
      {
         if (origin == null)
         {
            throw new ArgumentNullException("origin");
         }
         if (destination == null)
         {
            throw new ArgumentNullException("destination");
         }
         if (origin.HasSameIdentityAs(destination))
         {
            throw new ArgumentException("Origin and destination can't be the same.");
         }

         _origin = origin;
         _arrivalDeadline = arrivalDeadline;
         _destination = destination;
      }

      public DateTime ArrivalDeadline
      {
         get { return _arrivalDeadline; }
      }

      public Location.Location Destination
      {
         get { return _destination; }
      }

      public Location.Location Origin
      {
         get { return _origin; }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _origin;
         yield return _destination;
         yield return _arrivalDeadline;
      }

      /// <summary>
      /// For NHibernate.
      /// </summary>
      protected RouteSpecification()
      {         
      }
   }
}
