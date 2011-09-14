using DDDSample.Domain;
using Microsoft.Practices.ServiceLocation;

namespace DDDSample.Application
{
   /// <summary>
   /// Provides logic for raising and handling domain events.
   /// </summary>
   public class SimpleEventPublisher : IEventPublisher
   {
       public void Raise<T>(T eventArgs)
       {
           var registeredHandlers = ServiceLocator.Current.GetAllInstances<IEventHandler<T>>();
           foreach (var handler in registeredHandlers)
           {
               handler.Handle(eventArgs);
           }
       }
   }
}
