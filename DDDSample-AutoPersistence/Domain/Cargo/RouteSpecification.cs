using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Contains information about a route: its origin, destination and arrival deadline.
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class RouteSpecification : ValueObject, IAggregateMember<Cargo>
#pragma warning restore 661,660
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
         if (origin == destination)
         {
            throw new ArgumentException("Origin and destination can't be the same.");
         }

         _origin = origin;
         _arrivalDeadline = arrivalDeadline;
         _destination = destination;
      }

      /// <summary>
      /// Checks whether provided itinerary (a description of transporting steps) satisfies this
      /// specification.
      /// </summary>
      /// <param name="itinerary">An itinerary.</param>
      /// <returns>True, if cargo can be transported from <see cref="Origin"/> to <see cref="Destination"/>
      /// before <see cref="ArrivalDeadline"/> using provided itinerary.
      /// </returns>
      public virtual bool IsSatisfiedBy(Itinerary itinerary)
      {
         return Origin == itinerary.InitialDepartureLocation &&
                Destination == itinerary.FinalArrivalLocation &&
                ArrivalDeadline > itinerary.FinalArrivalDate;         
      }

      /// <summary>
      /// Date of expected cargo arrival.
      /// </summary>
      public DateTime ArrivalDeadline
      {
         get { return _arrivalDeadline; }
      }

      /// <summary>
      /// Location where cargo should be delivered.
      /// </summary>
      public Location.Location Destination
      {
         get { return _destination; }
      }

      /// <summary>
      /// Location where cargo should be picked up.
      /// </summary>
      public Location.Location Origin
      {
         get { return _origin; }
      }

      public static bool operator ==(RouteSpecification left, RouteSpecification right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(RouteSpecification left, RouteSpecification right)
      {
         return NotEqualOperator(left, right);
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
