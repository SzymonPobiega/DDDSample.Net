using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Specifies steps required to transport a cargo from its origin to destination.
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   [Serializable]
   public class Itinerary : ValueObject
#pragma warning restore 661,660
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
      public virtual UnLocode InitialDepartureLocation
      {
         get { return IsEmpty ? Location.Location.Unknown.UnLocode : _legs.First().LoadLocation; }
      }

      /// <summary>
      /// Gets the location of last arrival according to this itinerary.
      /// </summary>
      public virtual UnLocode FinalArrivalLocation
      {
         get { return IsEmpty ? Location.Location.Unknown.UnLocode : _legs.Last().UnloadLocation; }         
      }

      /// <summary>
      /// Gets the time of last arrival according to this itinerary. Returns null for empty itinerary.
      /// </summary>
      public virtual DateTime? FinalArrivalDate
      {
         get { return IsEmpty ? (DateTime?)null : _legs.Last().UnloadDate; }         
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