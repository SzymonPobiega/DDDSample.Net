using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Pathfinder
{
   /// <summary>
   /// Represents an edge in voyage graph.
   /// </summary>
   public sealed class TransitEdge
   {
      private readonly object _key;
      private readonly string _from;
      private readonly string _to;
      private readonly DateTime _fromDate;
      private readonly DateTime _toDate;

      /// <summary>
      /// Creates new <see cref="TransitEdge"/> object.
      /// </summary>
      /// <param name="key">User-specified key / id</param>
      /// <param name="from">Origin UnLocode string.</param>
      /// <param name="to">Destination UnLocode string.</param>
      /// <param name="fromDate">Depatrure date.</param>
      /// <param name="toDate">Arrival date.</param>
      public TransitEdge(object key, string from, string to, DateTime fromDate, DateTime toDate)
      {         
         _from = from;
         _key = key;
         _to = to;
         _fromDate = fromDate;
         _toDate = toDate;
      }

      /// <summary>
      /// Gets arrival date.
      /// </summary>
      public DateTime ToDate
      {
         get { return _toDate; }
      }

      /// <summary>
      /// Gets departure date.
      /// </summary>
      public DateTime FromDate
      {
         get { return _fromDate; }
      }

      /// <summary>
      /// Gets origin UnLocode string.
      /// </summary>
      public string To
      {
         get { return _to; }
      }

      /// <summary>
      /// Gets destination UnLocode string.
      /// </summary>
      public string From
      {
         get { return _from; }
      }

      /// <summary>
      /// Gets user-specified key / id.
      /// </summary>
      public object Key
      {
         get { return _key; }
      }
   }
}