using System;
using System.Collections.Generic;
using System.Linq;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Potential.Location;

namespace DDDSample.DomainModel.Operations.Cargo
{
#pragma warning disable 660,661
   public class Itinerary : ValueObject
#pragma warning restore 660,661
   {
      private readonly IList<Leg> _legs;

      /// <summary>
      /// Creates new <see cref="Itinerary"/> instance for provided collection of routing steps (legs).
      /// </summary>
      /// <param name="legs">Collection of routing steps (legs).</param>
      public Itinerary(IEnumerable<Leg> legs)
      {
         _legs = new List<Leg>(legs);
      }

      /// <summary>
      /// Gets unmodifiable collection of this itinerary's legs.
      /// </summary>
      public virtual IEnumerable<Leg> Legs
      {
         get { return _legs; }
      }

      /// <summary>
      /// Gets the location of first departure according to this itinerary.
      /// </summary>
      public virtual Location InitialDepartureLocation
      {
         get { return IsEmpty ? Location.Unknown : _legs.First().LoadLocation; }
      }

      /// <summary>
      /// Gets the location of last arrival according to this itinerary.
      /// </summary>
      public virtual Location FinalArrivalLocation
      {
         get { return IsEmpty ? Location.Unknown : _legs.Last().UnloadLocation; }         
      }

      /// <summary>
      /// Gets the time of last arrival according to this itinerary. Returns null for empty itinerary.
      /// </summary>
      public virtual DateTime? FinalArrivalDate
      {
         get { return IsEmpty ? (DateTime?)null : _legs.Last().UnloadDate; }         
      }

      /// <summary>
      /// Calculates total cost of this itinerary.
      /// </summary>
      /// <returns>Total cost.</returns>
      public virtual decimal CalculateTotalCost()
      {
         return _legs.Sum(x => x.CalculateCost());
      }

      /// <summary>
      /// Checks whether provided event is expected according to this itinerary specification.
      /// </summary>
      /// <param name="event">A handling event.</param>
      /// <returns>True, if it is expected. Otherwise - false. If itinerary is empty, returns false.</returns>
      public virtual bool IsExpected(HandlingEvent @event)
      {
         if (IsEmpty)
         {
            return false;
         }
         if (@event.EventType == HandlingEventType.Receive)
         {
            Leg firstLeg = _legs.First();
            return firstLeg.LoadLocation == @event.Location;
         }
         if (@event.EventType == HandlingEventType.Claim)
         {
            Leg lastLeg = _legs.Last();
            return lastLeg.UnloadLocation == @event.Location;
         }
         if (@event.EventType == HandlingEventType.Load)
         {
            return _legs.Any(x => x.LoadLocation == @event.Location);            
         }
         if (@event.EventType == HandlingEventType.Unload)
         {
            return _legs.Any(x => x.UnloadLocation == @event.Location);
         }
         //@event.EventType == HandlingEventType.Customs
         return true;
      }

      private bool IsEmpty
      {
         get { return _legs.Count() == 0; }
      }

      #region Infrastructure
      protected override IEnumerable<object> GetAtomicValues()
      {
         foreach (Leg leg in _legs)
         {
            yield return leg;
         }
      }

      public static bool operator ==(Itinerary left, Itinerary right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(Itinerary left, Itinerary right)
      {
         return NotEqualOperator(left, right);
      }

      protected Itinerary()
      {         
      }
      #endregion      
   }
}