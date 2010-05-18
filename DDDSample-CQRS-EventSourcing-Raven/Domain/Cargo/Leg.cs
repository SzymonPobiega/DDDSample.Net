using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{   
   /// <summary>
   /// Represents one step of an itinerary.
   /// </summary>
   [Serializable]
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class Leg : ValueObject
#pragma warning restore 661,660
   {
      /// <summary>
      /// Creates new leg instance.
      /// </summary>
      /// <param name="loadLocation">Location where cargo is supposed to be loaded.</param>
      /// <param name="loadDate">Date and time when cargo is supposed to be loaded</param>
      /// <param name="unloadLocation">Location where cargo is supposed to be unloaded.</param>
      /// <param name="unloadDate">Date and time when cargo is supposed to be unloaded.</param>
      public Leg(UnLocode loadLocation, DateTime loadDate, UnLocode unloadLocation, DateTime unloadDate)
      {
         LoadLocation = loadLocation;
         UnloadDate = unloadDate;
         UnloadLocation = unloadLocation;
         LoadDate = loadDate;
      }

      /// <summary>
      /// Gets location where cargo is supposed to be loaded.
      /// </summary>
      public UnLocode LoadLocation { get; private set; }

      /// <summary>
      /// Gets location where cargo is supposed to be unloaded.
      /// </summary>
      public UnLocode UnloadLocation { get; private set; }

      /// <summary>
      /// Gets date and time when cargo is supposed to be loaded.
      /// </summary>
      public DateTime LoadDate { get; private set; }

      /// <summary>
      /// Gets date and time when cargo is supposed to be unloaded.
      /// </summary>
      public DateTime UnloadDate { get; private set; }

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
