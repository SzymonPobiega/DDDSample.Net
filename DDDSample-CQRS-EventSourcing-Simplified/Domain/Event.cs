using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain
{
   /// <summary>
   /// Represents base class for domain events.
   /// </summary>
   /// <typeparam name="T">Aggregate root type to which event applies.</typeparam>
   [Serializable]
   public abstract class Event<T>
      where T : AggregateRoot
   {      
   }
}