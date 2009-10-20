using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain
{
   /// <summary>
   /// Base class for all domain events.
   /// </summary>
   /// <typeparam name="T">Type of object publishing events.</typeparam>
   public abstract class DomainEvent<T>
   {
      private readonly T _source;

      protected DomainEvent(T source)
      {
         _source = source;
      }

      /// <summary>
      /// Gets object which is the source of the event.
      /// </summary>
      public T Source
      {
         get { return _source; }
      }
   }
}