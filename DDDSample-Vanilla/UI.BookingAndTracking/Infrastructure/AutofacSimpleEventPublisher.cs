using System.Collections.Generic;
using Autofac;
using DDDSample.Domain;

namespace DDDSample.UI.BookingAndTracking.Infrastructure
{
   /// <summary>
   /// Provides logic for raising and handling domain events.
   /// </summary>
   public class AutofacSimpleEventPublisher : IEventPublisher
   {
       private readonly IContainer _container;

       public AutofacSimpleEventPublisher(IContainer container)
       {
           _container = container;
       }

       public void Raise<T>(T eventArgs)
       {
           var registeredHandlers = _container.Resolve<IEnumerable<IEventHandler<T>>>();
           foreach (var handler in registeredHandlers)
           {
               handler.Handle(eventArgs);
           }
       }
   }
}
