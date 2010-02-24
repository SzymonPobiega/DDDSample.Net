using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace DDDSample.Domain
{
   public abstract class AggregateRoot
   {
      protected void Apply(object @event)
      {
         if (@event == null)
         {
            throw new ArgumentNullException("event");
         }
         string eventTypeName = @event.GetType().Name;
         int suffixIndex = eventTypeName.LastIndexOf("Event");
         if (suffixIndex <= 0)
         {
            throw new InvalidOperationException("Invalid event name: " + eventTypeName);
         }
         string methodName = "On" + eventTypeName.Substring(0, suffixIndex);
         MethodInfo methodInfo = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
         methodInfo.Invoke(this, new[] { @event });
      }

      protected void Publish(object @event)
      {
         Apply(@event);
         ServiceLocator.Current.GetInstance<IBus>().Publish(@event);
      }

      public void LoadFromEventStream(IEnumerable<object> events)
      {
         foreach (object @event in events)
         {
            Apply(@event);
         }
      }
   }
}