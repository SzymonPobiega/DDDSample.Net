using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{   
   /// <summary>
   /// Represents one step of an itinerary.
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class Leg : ValueObject, IAggregateMember<Cargo>
#pragma warning restore 661,660
   {
      private readonly Location.Location _loadLocation;
      private readonly Location.Location _unloadLocation;

      private readonly DateTime _loadDate;
      private readonly DateTime _unloadDate;

      /// <summary>
      /// Creates new leg instance.
      /// </summary>
      /// <param name="loadLocation">Location where cargo is supposed to be loaded.</param>
      /// <param name="loadDate">Date and time when cargo is supposed to be loaded</param>
      /// <param name="unloadLocation">Location where cargo is supposed to be unloaded.</param>
      /// <param name="unloadDate">Date and time when cargo is supposed to be unloaded.</param>
      public Leg(Location.Location loadLocation, DateTime loadDate, Location.Location unloadLocation, DateTime unloadDate)
      {
         _loadLocation = loadLocation;
         _unloadDate = unloadDate;
         _unloadLocation = unloadLocation;
         _loadDate = loadDate;
      }

      /// <summary>
      /// Gets location where cargo is supposed to be loaded.
      /// </summary>
      public Location.Location LoadLocation
      {
         get { return _loadLocation; }
      }

      /// <summary>
      /// Gets location where cargo is supposed to be unloaded.
      /// </summary>
      public Location.Location UnloadLocation
      {
         get { return _unloadLocation; }
      }

      /// <summary>
      /// Gets date and time when cargo is supposed to be loaded.
      /// </summary>
      public DateTime LoadDate
      {
         get { return _loadDate; }
      }

      /// <summary>
      /// Gets date and time when cargo is supposed to be unloaded.
      /// </summary>
      public DateTime UnloadDate
      {
         get { return _unloadDate; }
      }

      #region Infrastructure
      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _loadLocation;
         yield return _unloadLocation;
         yield return _loadDate;
         yield return _unloadDate;
      }

      public static bool operator ==(Leg left, Leg right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(Leg left, Leg right)
      {
         return NotEqualOperator(left, right);
      }

      protected Leg()
      {
      }
      #endregion
   }   
}
