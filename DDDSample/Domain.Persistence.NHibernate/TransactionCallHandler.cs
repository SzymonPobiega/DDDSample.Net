using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using NHibernate;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Call handler for wrapping service layer calls into NHibernate transactions.
   /// </summary>
   public class TransactionCallHandler : ICallHandler
   {
      private readonly ISessionFactory _sessionFactory;

      public TransactionCallHandler(ISessionFactory sessionFactory)
      {
         _sessionFactory = sessionFactory;
      }

      public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
      {
         using (ITransaction tx = _sessionFactory.GetCurrentSession().BeginTransaction())
         {
            IMethodReturn result = getNext()(input, getNext);
            if (result.Exception == null)
            {
               tx.Commit();
            }
            return result;
         }
      }

      public int Order { get; set;}      
   }
}
