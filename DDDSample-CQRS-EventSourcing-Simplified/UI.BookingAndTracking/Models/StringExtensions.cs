using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.UI.BookingAndTracking.Models
{
   /// <summary>
   /// String extensions for easy GUI programming.
   /// </summary>
   public static class StringExtensions
   {
      /// <summary>
      /// Returns string formatted using this format string and provided arguments.
      /// </summary>
      /// <param name="formatString"></param>
      /// <param name="arguments"></param>
      /// <returns></returns>
      public static string UIFormat(this string formatString, params object[] arguments)
      {
         return string.Format(CultureInfo.CurrentCulture, formatString, arguments);
      }
   }
}