using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain
{
   public abstract class ValueObject      
   {
      protected static bool EqualOperator(ValueObject left, ValueObject right)
      {
         if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
         {
            return false;
         }
         return ReferenceEquals(left, null) || left.Equals(right);
      }

      protected static bool NotEqualOperator(ValueObject left, ValueObject right)
      {
         return !(left == right);
      }
   }
}
