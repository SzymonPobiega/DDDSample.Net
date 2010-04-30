using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Environment=NHibernate.Cfg.Environment;

namespace Domain.Persistence.Tests
{
   /// <summary>
   /// Base class for all persistence tests.
   /// </summary>
   [TestFixture]
   public abstract class PersistenceTest
   {
      private string DatabaseFile;
      protected ISessionFactory SessionFactory { get; private set; }      
      protected ISession Session { get { return SessionFactory.GetCurrentSession(); } }
      protected ITransaction Transaction { get { return SessionFactory.GetCurrentSession().Transaction; } }

      [SetUp]
      public void Initialize()
      {                  
         DatabaseFile = GetDbFileName();
         EnsureDbFileNotExists();
         
         Configuration cfg = new Configuration()
             .AddProperties(new Dictionary<string, string>
                               {
                                   { Environment.ConnectionDriver, typeof( SQLite20Driver ).FullName },
                                   { Environment.Dialect, typeof( SQLiteDialect ).FullName },
                                   { Environment.ConnectionProvider, typeof( DriverConnectionProvider ).FullName },
                                   { Environment.ConnectionString, string.Format( "Data Source={0};Version=3;New=True;", DatabaseFile) },
                                   { Environment.ProxyFactoryFactoryClass, typeof( ProxyFactoryFactory ).AssemblyQualifiedName },
                                   { Environment.CurrentSessionContextClass, typeof( ThreadStaticSessionContext ).AssemblyQualifiedName },
                                   { Environment.Hbm2ddlAuto, "create" },
                                   { Environment.ShowSql, true.ToString() }
                               });
         cfg.AddAssembly("DDDSample.Domain.Persistence.NHibernate");         
         SessionFactory = cfg.BuildSessionFactory();
      }

      [TearDown]
      public void TearDownTests()
      {
         SessionFactory.Dispose();
         EnsureDbFileNotExists();         
      }

      private static string GetDbFileName()
      {
         return Path.GetFullPath(Guid.NewGuid().ToString("N") + ".Test.db");         
      }

      private void EnsureDbFileNotExists()
      {
         if (File.Exists(DatabaseFile))
         {
            File.Delete(DatabaseFile);
         }
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
            CurrentSessionContext.Bind(session);
         }

         public void Dispose()
         {
            ISession session = CurrentSessionContext.Unbind(_sessionFactory);
            if (!IsInExceptionContext())
            {               
               if (session.Transaction != null)
               {
                  session.Transaction.Commit();
                  session.Transaction.Dispose();
               }               
            }
            session.Close();            
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