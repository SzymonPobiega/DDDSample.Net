using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace DDDSample.Domain
{
   /// <summary>
   /// Provides logic for raising and handling domain events.
   /// </summary>
   public static class Bus
   {
      [ThreadStatic]
      private static List<Delegate> _actions;
      private static List<Delegate> Actions
      {
         get
         {
            if (_actions == null)
            {
               _actions = new List<Delegate>();
            }
            return _actions;
         }
      }

      /// <summary>
      /// Rejestruje prcedure obsługi zdarzenia.
      /// </summary>
      /// <param name="callback">Procedura osbługi zdarzenia.</param>
      /// <returns></returns>
      public static IDisposable Register<TAggregate, TEvent>(Action<TAggregate, TEvent> callback)
         where TAggregate : AggregateRoot
         where TEvent : Event<TAggregate>
      {
         Actions.Add(callback);
         return new DomainEventRegistrationRemover(() => Actions.Remove(callback));
      }

      /// <summary>
      /// Sygnalizuje zdarzenie.
      /// </summary>
      public static void Publish<TAggregate, TEvent>(TAggregate source, TEvent @event)
         where TAggregate : AggregateRoot
         where TEvent : Event<TAggregate>
      {
         IEnumerable<IEventHandler<TAggregate, TEvent>> registeredHandlers = ServiceLocator.Current.GetAllInstances<IEventHandler<TAggregate, TEvent>>();
         foreach (IEventHandler<TAggregate, TEvent> handler in registeredHandlers)
         {
            handler.Handle(source, @event);
         }
         foreach (Delegate action in Actions)
         {
            Action<TAggregate, TEvent> typedAction = action as Action<TAggregate, TEvent>;
            if (typedAction != null)
            {
               typedAction(source, @event);
            }
         }
      }
      
      /// <summary>
      /// Klasa pomocnicza.
      /// </summary>
      private sealed class DomainEventRegistrationRemover : IDisposable
      {
         private readonly Action _callOnDispose;

         public DomainEventRegistrationRemover(Action toCall)
         {
            _callOnDispose = toCall;
         }

         public void Dispose()
         {
            _callOnDispose();
         }
      }
   }
}
