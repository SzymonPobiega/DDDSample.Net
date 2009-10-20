using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// Defines constraints for finding paths.
   /// </summary>
   public class Constraints
   {
      private readonly DateTime _arrivalDeadline;

      /// <summary>
      /// Creates new constraints object.
      /// </summary>
      /// <param name="arrivalDeadline">Arrival deadline.</param>
      public Constraints(DateTime arrivalDeadline)
      {
         _arrivalDeadline = arrivalDeadline;
      }

      /// <summary>
      /// Gets arrival deadline.
      /// </summary>
      public DateTime ArrivalDeadline
      {
         get { return _arrivalDeadline; }
      }
   }
}