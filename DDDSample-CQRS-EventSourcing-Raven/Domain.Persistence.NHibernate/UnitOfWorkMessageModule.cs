using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using NHibernate;
using NServiceBus;
using Raven.Client;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class UnitOfWorkMessageModule : IMessageModule
   {
      private readonly IDocumentStore _documentStore;

      public UnitOfWorkMessageModule(IDocumentStore documentStore)
      {
         _documentStore = documentStore;
      }

      public void HandleBeginMessage()
      {
         UnitOfWork.Current = new UnitOfWork(_documentStore);         
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