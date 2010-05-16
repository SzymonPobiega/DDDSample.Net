using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Context;
using NServiceBus;

namespace DDDSample.Application.AsynchronousEventHandlers.MessageHandlers
{
   /// <summary>
   /// Base class for message handlers.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public abstract class AbstractMessageHandler<T> : IHandleMessages<T>
      where T : IMessage
   {
      private readonly ISessionFactory _sessionFactory;

      protected AbstractMessageHandler(ISessionFactory sessionFactory)
      {
         _sessionFactory = sessionFactory;
      }

      public void Handle(T message)
      {
         ISession session = _sessionFactory.OpenSession();
         CurrentSessionContext.Bind(session);
         ITransaction trans = session.BeginTransaction();
         try
         {
            DoHandle(message);
            trans.Commit();
         }         
         finally
         {            
            CurrentSessionContext.Unbind(_sessionFactory);
            session.Dispose();
         }         
      }

      protected abstract void DoHandle(T message);
   }
}