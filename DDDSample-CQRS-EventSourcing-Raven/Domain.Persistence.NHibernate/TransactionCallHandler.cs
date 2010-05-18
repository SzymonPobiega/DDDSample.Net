using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using Microsoft.Practices.Unity.InterceptionExtension;
using NHibernate;
using NHibernate.Context;
using Raven.Client;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Call handler for wrapping service layer calls into NHibernate transactions.
   /// </summary>
   public class TransactionCallHandler : ICallHandler
   {
      private readonly IDocumentStore _documentStore;

      public TransactionCallHandler(IDocumentStore documentStore)
      {
         _documentStore = documentStore;
      }

      public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
      {
         Debug.Assert(UnitOfWork.Current == null);
         UnitOfWork.Current = new UnitOfWork(_documentStore);

         IMethodReturn result = getNext()(input, getNext);

         if (result.Exception == null)
         {
            UnitOfWork.Current.Commit();
         }
         UnitOfWork.Current.Rollback();
         UnitOfWork.Current = null;         
         return result;      
      }

      public int Order { get; set;}      
   }
}
