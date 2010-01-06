using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using Microsoft.Practices.Unity.InterceptionExtension;
using NHibernate;
using NHibernate.Context;

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
         IMethodReturn result;
         SqlConnection sqlConnection = _sessionFactory.GetCurrentSession().Connection as SqlConnection;
         using (TransactionScope tx = new TransactionScope())
         {            
            if (sqlConnection == null)
            {
               throw new NotSupportedException("Only SqlConnection is supported.");
            }
            sqlConnection.EnlistTransaction(Transaction.Current);
            result = getNext()(input, getNext);
            if (result.Exception == null)
            {
               _sessionFactory.GetCurrentSession().Flush();
               tx.Complete();
            }                        
         }
         sqlConnection.EnlistTransaction(null);
         return result;         
      }

      private ISession BindNHibernateSession()
      {
         ISession old = ManagedWebSessionContext.Unbind(HttpContext.Current, _sessionFactory);
         ManagedWebSessionContext.Bind(HttpContext.Current, _sessionFactory.OpenSession());
         return old;
      }

      private void UnbindNHibernateSession(ISession sessionToRestore)
      {
         ManagedWebSessionContext.Unbind(HttpContext.Current, _sessionFactory).Dispose();
         ManagedWebSessionContext.Bind(HttpContext.Current, sessionToRestore);
      }

      public int Order { get; set;}      
   }
}
