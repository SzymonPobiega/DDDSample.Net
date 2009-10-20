using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// 
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
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
      public IEnumerable<Leg> Legs
      {
         get { return _legs; }
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