using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Domain.Persistence.Tests
{
   /// <summary>
   /// Base class for all persistence tests.
   /// </summary>
   [TestFixture]
   public abstract class PersistenceTest
   {
      protected ISessionFactory SessionFactory { get; private set; }
      protected ISession Session { get { return SessionFactory.GetCurrentSession(); } }
      protected ITransaction Transaction { get { return SessionFactory.GetCurrentSession().Transaction; } }

      [SetUp]
      public void Initialize()
      {
         Configuration cfg = new Configuration().Configure();
         new SchemaExport(cfg).Execute(false, true, false);
         SessionFactory = cfg.BuildSessionFactory();         
      }

      protected IDisposable Scope(bool transactional)
      {
         return new ScopeImpl(SessionFactory, transactional);
      }

      protected IDisposable Scope(bool transactional, string description)
      {
         Console.WriteLine(description);
         return Scope(transactional);
      }

      private class ScopeImpl : IDisposable
      {
         private readonly ISessionFactory _sessionFactory;

         public ScopeImpl(ISessionFactory sessionFactory, bool transactional)
         {
            _sessionFactory = sessionFactory;
            ISession session = _sessionFactory.OpenSession();
            if (transactional)
            {
               session.BeginTransaction();
            }
            NHibernate.Context.CurrentSessionContext.Bind(session);
         }

         public void Dispose()
         {
            ISession session = NHibernate.Context.CurrentSessionContext.Unbind(_sessionFactory);
            if (!IsInExceptionContext())
            {               
               if (session.Transaction != null)
               {
                  session.Transaction.Commit();
                  session.Transaction.Dispose();
               }
               session.Dispose();
            }
         }

         /// <summary>
         /// Checks if current code is running in finally block ater throwing exception.
         /// </summary>         
         private static Boolean IsInExceptionContext()
         {
            return Marshal.GetExceptionPointers() != IntPtr.Zero || Marshal.GetExceptionCode() != 0;
         }
      }
   }
}