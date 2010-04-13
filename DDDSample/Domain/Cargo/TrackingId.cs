using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.DomainModel.Operations.Cargo
{
   public class TrackingId : ValueObject
#pragma warning restore 661,660
   {
      private readonly string _idString;

      /// <summary>
      /// Creates new <see cref="TrackingId"/> instacnce.
      /// </summary>
      /// <param name="idString">String representation of this new tracking id.</param>
      public TrackingId(string idString)
      {
         _idString = idString;
      }

      /// <summary>
      /// Returns string representation of this tracking id.
      /// </summary>
      public virtual string IdString
      {
         get { return _idString; }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _idString;
      }

      public static bool operator == (TrackingId left, TrackingId right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(TrackingId left, TrackingId right)
      {
         return NotEqualOperator(left, right);
      }

      /// <summary>
      /// For NHibernate.
      /// </summary>
      protected TrackingId()
      {         
      }
   }
}