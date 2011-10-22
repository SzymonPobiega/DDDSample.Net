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
         using (TransactionScope tx = new TransactionScope())
         {                        
            result = getNext()(input, getNext);
            if (result.Exception == null)
            {               
               _sessionFactory.GetCurrentSession().Flush();
               tx.Complete();
            }                        
         }
         CurrentSessionContext.Bind(_sessionFactory.OpenSession());
         return result;         
      }
      
      public int Order { get; set;}      
   }
}
