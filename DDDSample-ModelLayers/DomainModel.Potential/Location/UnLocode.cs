using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDDSample.DomainModel.Potential.Location
{
   public class UnLocode : ValueObject
#pragma warning restore 661,660
   {
      private static readonly Regex _codePattern = new Regex("[a-zA-Z]{2}[a-zA-Z2-9]{3}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
      private readonly string _code;

      /// <summary>
      /// Creates new <see cref="UnLocode"/> object.
      /// </summary>
      /// <param name="code">String representation of location code.</param>
      public UnLocode(string code)
      {
         if(code == null)
         {
            throw new ArgumentNullException("code");
         }
         if (!_codePattern.Match(code).Success)
         {
            throw new ArgumentException(string.Format("Provided code does not comply with a UnLocode pattern ({0})",_codePattern), "code");
         }
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

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _code;
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

      public override string ToString()
      {
         return _code;
      }
   }
}