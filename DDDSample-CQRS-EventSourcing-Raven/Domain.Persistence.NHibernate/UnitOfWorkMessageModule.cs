using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using NHibernate;
using NServiceBus;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class UnitOfWorkMessageModule : IMessageModule
   {
      private readonly ISessionFactory _sessionFactory;

      public UnitOfWorkMessageModule(ISessionFactory sessionFactory)
      {
         _sessionFactory = sessionFactory;
      }

      public void HandleBeginMessage()
      {
         UnitOfWork.Current = new UnitOfWork(_sessionFactory);         
      }

      public void HandleEndMessage()
      {
         UnitOfWork.Current.Commit();
         UnitOfWork.Current = null;
      }

      public void HandleError()
      {         
      }
   }
}