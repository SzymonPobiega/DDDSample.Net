using System.Collections.Generic;
using System.Linq;
using DDDSample.Domain;

namespace DDDSample.DomainModel.Operations.Handling
{
#pragma warning disable 660,661
    /// <summary>
   /// Contains information about cargo handling history. 
   /// </summary>
   public class HandlingHistory : ValueObject
#pragma warning restore 660,661
   {
      private readonly IList<HandlingEvent> _events;

      public HandlingHistory(IEnumerable<HandlingEvent> events)
      {         
         _events = new List<HandlingEvent>(events);
      }
          

      /// <summary>
      /// Gets a collection of events ordered by their completion time.
      /// </summary>
      public virtual IEnumerable<HandlingEvent> EventsByCompletionTime
      {
         get { return _events.OrderBy(x => x.CompletionDate);}
      }

      public static bool operator ==(HandlingHistory left, HandlingHistory right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(HandlingHistory left, HandlingHistory right)
      {
         return NotEqualOperator(left, right);
      }
            
      protected override IEnumerable<object> GetAtomicValues()
      {
         return _events.Cast<object>();
      }
   }
}
