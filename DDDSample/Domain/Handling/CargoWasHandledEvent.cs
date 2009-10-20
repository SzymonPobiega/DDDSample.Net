using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Handling
{
   /// <summary>
   /// Signals that a cargo was handled.
   /// </summary>
   public class CargoWasHandledEvent
   {
      private readonly HandlingEvent _data;

      /// <summary>
      /// Creates new event instance.
      /// </summary>
      /// <param name="data"></param>
      public CargoWasHandledEvent(HandlingEvent data)
      {
         _data = data;
      }

      /// <summary>
      /// Data associated with this event.
      /// </summary>
      public HandlingEvent Data
      {
         get { return _data; }
      }
   }
}
