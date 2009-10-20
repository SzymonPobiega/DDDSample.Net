using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Voyage
{
   /// <summary>
   /// Represents a unique voyage number.
   /// </summary>
#pragma warning disable 661,660
   public class VoyageNumber : ValueObject
#pragma warning restore 661,660
   {
// ReSharper disable FieldCanBeMadeReadOnly.Local
      private string _number;
// ReSharper restore FieldCanBeMadeReadOnly.Local

      public VoyageNumber(string number)
      {
         if (number == null)
         {
            throw new ArgumentNullException("number");
         }
         _number = number;
      }


      public static bool operator ==(VoyageNumber left, VoyageNumber right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(VoyageNumber left, VoyageNumber right)
      {
         return NotEqualOperator(left, right);
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _number;
      }

      protected VoyageNumber()
      {         
      }
   }
}
