using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DDDSample.CommandHandlers;
using DDDSample.Commands;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using DDDSample.Reporting.Persistence.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using NHibernate;
using NHibernate.ByteCode.LinFu;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NServiceBus;
using NUnit.Framework;
using Environment=NHibernate.Cfg.Environment;

namespace Tests.Integration
{
   [TestFixture]
   public abstract class ScenarioTest
   {
      public static UnLocode HONGKONG
      {
         get { return new UnLocode("CNHKG"); }
      }
      public static UnLocode STOCKHOLM
      {
         get { return new UnLocode("SESTO"); }
      }
      public static UnLocode TOKYO
      {
         get { return new UnLocode("JNTKO"); }
      }
      public static UnLocode HAMBURG
      {
         get { return new UnLocode("DEHAM"); }
      }
      public static UnLocode NEWYORK
      {
         get { return new UnLocode("USNYC"); }
      }
      public static UnLocode CHICAGO
      {
         get { return new UnLocode("USCHI"); }
      }
      public static UnLocode GOETEBORG
      {
         get { return new UnLocode("SEGOT"); }
      }

      public static ICargoRepository CargoRepository
      {
         get { return ServiceLocator.Current.GetInstance<ICargoRepository>(); }
      }

      public static ILocationRepository LocationRepository
      {
         get { return ServiceLocator.Current.GetInstance<ILocationRepository>(); }
      }

      public static CargoDataAccess CargoDataAccess
      {
         get { return ServiceLocator.Current.GetInstance<CargoDataAccess>(); }
      }

      private static IServiceLocator _ambientLocator;
      private static IUnityContainer _ambientContainer;
      protected static ISessionFactory _sessionFactory;

      private string DatabaseFile;
         
      [SetUp]
      public void Initialize()
      {
         DatabaseFile = GetDbFileName();
         EnsureDbFileNotExists();

         _ambientContainer = new UnityContainer();

         ConfigureNHibernateRepositories();

         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(() => _ambientLocator);     
    
         InitializeNHibernate();
      }

      [TearDown]
      public void TearDownTests()
      {
         _sessionFactory.Dispose();
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

      private static void ConfigureNHibernateRepositories()
      {
         _ambientContainer.RegisterType<ILocationRepository, LocationRepository>();
         _ambientContainer.RegisterType<ICargoRepository, CargoRepository>();

         _ambientContainer.RegisterType<IMessageHandler<BookNewCargoCommand>, BookNewCargoCommandHandler>();
         _ambientContainer.RegisterType<IMessageHandler<AssignCargoToRouteCommand>, AssignCargoToRouteCommandHandler>();
         _ambientContainer.RegisterType<IMessageHandler<ChangeDestinationCommand>, ChangeDestinationCommandHandler>();
         _ambientContainer.RegisterType<IMessageHandler<RegisterHandlingEventCommand>, RegisterHandlingEventCommandHandler>();

         //_ambientContainer.AddNewExtension<Interception>();

         //_ambientContainer.Configure<Interception>()

         //   .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
         //   .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())

         //   .AddPolicy("Transactions")
         //   .AddCallHandler<TransactionCallHandler>()
         //   .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));         
      }

      private void InitializeNHibernate()
      {         
         Configuration cfg = new Configuration()
            .AddProperties(new Dictionary<string, string>
                              {
                                 {Environment.ConnectionDriver, typeof (SQLite20Driver).FullName},
                                 {Environment.Dialect, typeof (SQLiteDialect).FullName},
                                 {Environment.ConnectionProvider, typeof (DriverConnectionProvider).FullName},
                                 { Environment.ConnectionString, string.Format( "Data Source={0};Version=3;New=True;", DatabaseFile) },
                                 {
                                    Environment.ProxyFactoryFactoryClass,
                                    typeof (ProxyFactoryFactory).AssemblyQualifiedName
                                    },
                                 {
                                    Environment.CurrentSessionContextClass,
                                    typeof (ThreadStaticSessionContext).AssemblyQualifiedName
                                    },                                 
                                 {Environment.Hbm2ddlAuto, "create"},
                                 {Environment.ShowSql, true.ToString()}
                              });
         cfg.AddAssembly("DDDSample.Domain.Persistence.NHibernate");

         _sessionFactory = cfg.BuildSessionFactory();
         _ambientContainer.RegisterInstance(_sessionFactory);         

         new SchemaExport(cfg).Execute(false, true, false);

         ISession ambientSession = _sessionFactory.OpenSession();
         CurrentSessionContext.Bind(ambientSession);
      }
   }
}
