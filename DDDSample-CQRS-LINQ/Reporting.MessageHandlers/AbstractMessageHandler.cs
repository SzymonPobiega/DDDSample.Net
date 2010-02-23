using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using NServiceBus;

namespace DDDSample.Reporting.MessageHandlers
{
   /// <summary>
   /// Base class for message handlers.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public abstract class AbstractMessageHandler<T> : IHandleMessages<T>
      where T : IMessage
   {
      public void Handle(T message)
      {
         DoHandle(message);
      }

      protected abstract void DoHandle(T message);
   }
}