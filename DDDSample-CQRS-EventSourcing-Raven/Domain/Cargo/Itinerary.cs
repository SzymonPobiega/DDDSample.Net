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
      public IList<Leg> Legs { get; private set; }

      /// <summary>
      /// Creates new <see cref="Itinerary"/> instance for provided collection of routing steps (legs).
      /// </summary>
      /// <param name="legs">Collection of routing steps (legs).</param>
      public Itinerary(IEnumerable<Leg> legs)
      {
         Legs = new List<Leg>(legs);
      }
      
      /// <summary>
      /// Gets the location of first departure according to this itinerary.
      /// </summary>
      public virtual UnLocode InitialDepartureLocation
      {
         get { return IsEmpty ? Location.Location.Unknown.UnLocode : Legs.First().LoadLocation; }
      }

      /// <summary>
      /// Gets the location of last arrival according to this itinerary.
      /// </summary>
      public virtual UnLocode FinalArrivalLocation
      {
         get { return IsEmpty ? Location.Location.Unknown.UnLocode : Legs.Last().UnloadLocation; }         
      }

      /// <summary>
      /// Gets the time of last arrival according to this itinerary. Returns null for empty itinerary.
      /// </summary>
      public virtual DateTime? FinalArrivalDate
      {
         get { return IsEmpty ? (DateTime?)null : Legs.Last().UnloadDate; }         
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
            Leg firstLeg = Legs.First();
            return firstLeg.LoadLocation == @event.Location;
         }
         if (@event.EventType == HandlingEventType.Claim)
         {
            Leg lastLeg = Legs.Last();
            return lastLeg.UnloadLocation == @event.Location;
         }
         if (@event.EventType == HandlingEventType.Load)
         {
            return Legs.Any(x => x.LoadLocation == @event.Location);            
         }
         if (@event.EventType == HandlingEventType.Unload)
         {
            return Legs.Any(x => x.UnloadLocation == @event.Location);
         }
         //@event.EventType == HandlingEventType.Customs
         return true;
      }

      private bool IsEmpty
      {
         get { return Legs.Count() == 0; }
      }

      #region Infrastructure
      protected override IEnumerable<object> GetAtomicValues()
      {
         foreach (Leg leg in Legs)
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