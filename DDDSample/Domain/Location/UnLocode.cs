using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Location
{
   /// <summary>
   /// United nations location code.
   /// 
   /// http://www.unece.org/cefact/locode/
   /// http://www.unece.org/cefact/locode/DocColumnDescription.htm#LOCODE
   /// </summary>
   public class UnLocode : ValueObject
   {
      private readonly string _code;

      /// <summary>
      /// Creates new <see cref="UnLocode"/> object.
      /// </summary>
      /// <param name="code">String representation of location code.</param>
      public UnLocode(string code)
      {
         _code = code;
      }

      /// <summary>
      /// Returns a string representation of this UnLocode consisting of 5 characters (all upper):
      /// 2 chars of ISO country code and 3 describing location.
      /// </summary>
      public virtual string CodeString
      {
         get { return _code; }
      }

      public override bool Equals(object obj)
      {
         UnLocode other = obj as UnLocode;
         return other != null && _code.Equals(other._code);
      }

      public override int GetHashCode()
      {
         return _code.GetHashCode();
      }

      public static bool operator ==(UnLocode left, UnLocode right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(UnLocode left, UnLocode right)
      {
         return NotEqualOperator(left, right);
      }

      /// <summary>
      /// For NHibernate.
      /// </summary>
      protected UnLocode()
      {         
      }
   }
}
