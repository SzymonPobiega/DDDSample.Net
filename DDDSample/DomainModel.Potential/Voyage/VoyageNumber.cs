using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.DomainModel.Potential.Voyage
{
#pragma warning disable 660,661
   public class VoyageNumber : ValueObject
#pragma warning restore 660,661
   {
      private readonly string _number;

      /// <summary>
      /// Creates new voyage number using specified string.
      /// </summary>
      /// <param name="number">String represantation of voyage number.</param>
      public VoyageNumber(string number)
      {
         if (number == null)
         {
            throw new ArgumentNullException("number");
         }
         _number = number;
      }

      /// <summary>
      /// Gets string representation of this voyage number.
      /// </summary>
      public virtual string NumberString
      {
         get { return _number; }
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