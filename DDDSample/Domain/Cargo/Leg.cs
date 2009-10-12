using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// NHibernate does not allow fields to be read only.
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace DDDSample.Domain.Cargo
{   
   /// <summary>
   /// Represents one step of an itinerary.
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class Leg : ValueObject
#pragma warning restore 661,660
   {
      private Location.Location loadLocation;
      private Location.Location unloadLocation;

      private DateTime loadDate;
      private DateTime unloadDate;

      /// <summary>
      /// Creates new leg instance.
      /// </summary>
      /// <param name="loadLocation">Location where cargo is supposed to be loaded.</param>
      /// <param name="loadDate">Date and time when cargo is supposed to be loaded</param>
      /// <param name="unloadLocation">Location where cargo is supposed to be unloaded.</param>
      /// <param name="unloadDate">Date and time when cargo is supposed to be unloaded.</param>
      public Leg(Location.Location loadLocation, DateTime loadDate, Location.Location unloadLocation, DateTime unloadDate)
      {
         this.loadLocation = loadLocation;
         this.unloadDate = unloadDate;
         this.unloadLocation = unloadLocation;
         this.loadDate = loadDate;
      }

      /// <summary>
      /// Gets location where cargo is supposed to be loaded.
      /// </summary>
      public Location.Location LoadLocation
      {
         get { return loadLocation; }
      }

      /// <summary>
      /// Gets location where cargo is supposed to be unloaded.
      /// </summary>
      public Location.Location UnloadLocation
      {
         get { return unloadLocation; }
      }

      /// <summary>
      /// Gets date and time when cargo is supposed to be loaded.
      /// </summary>
      public DateTime LoadDate
      {
         get { return loadDate; }
      }

      /// <summary>
      /// Gets date and time when cargo is supposed to be unloaded.
      /// </summary>
      public DateTime UnloadDate
      {
         get { return unloadDate; }
      }

      #region Infrastructure
      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return LoadLocation;
         yield return UnloadLocation;
         yield return LoadDate;
         yield return UnloadDate;
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
// ReSharper restore FieldCanBeMadeReadOnly.Local
