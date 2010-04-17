using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.DomainModel
{
   /// <summary>
   /// Handles events of class <typeparamref name="T"/>.
   /// </summary>
   /// <typeparam name="T">Type of event.</typeparam>
   public interface IEventHandler<T>
   {
      /// <summary>
      /// Handles the event.
      /// </summary>
      /// <param name="event">Event object.</param>
      void Handle(T @event);
   }
}