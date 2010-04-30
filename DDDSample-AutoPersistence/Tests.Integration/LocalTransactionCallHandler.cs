using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using NHibernate;
using NHibernate.Context;

namespace Tests.Integration
{
   /// <summary>
   /// Call handler for wrapping service layer calls into NHibernate transactions.
   /// </summary>
   public class LocalTransactionCallHandler : ICallHandler
   {
      private readonly ISessionFactory _sessionFactory;

      public LocalTransactionCallHandler(ISessionFactory sessionFactory)
      {
         _sessionFactory = sessionFactory;
      }

      public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
      {
         IMethodReturn result;
         using (ITransaction tx = _sessionFactory.GetCurrentSession().BeginTransaction())
         {
            result = getNext()(input, getNext);
            if (result.Exception == null)
            {
               _sessionFactory.GetCurrentSession().Flush();
               tx.Commit();
            }
         }
         return result;
      }

      public int Order { get; set; }
   }
}