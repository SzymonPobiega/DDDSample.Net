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
   [Serializable]
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class RouteSpecification : ValueObject
#pragma warning restore 661,660
   {
      public RouteSpecification(UnLocode origin, UnLocode destination, DateTime arrivalDeadline)
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

         Origin = origin;
         ArrivalDeadline = arrivalDeadline;
         Destination = destination;
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
      public DateTime ArrivalDeadline { get; private set; }

      /// <summary>
      /// Location where cargo should be delivered.
      /// </summary>
      public UnLocode Destination { get; private set; }

      /// <summary>
      /// Location where cargo should be picked up.
      /// </summary>
      public UnLocode Origin { get; private set; }

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
         yield return Origin;
         yield return Destination;
         yield return ArrivalDeadline;
      }

      /// <summary>
      /// For NHibernate.
      /// </summary>
      protected RouteSpecification()
      {         
      }      
   }
}
