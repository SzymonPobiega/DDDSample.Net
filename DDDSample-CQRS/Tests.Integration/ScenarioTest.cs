using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DDDSample.Application;
using DDDSample.Application.Implemetation;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.EventHandlers;
using DDDSample.Domain.Location;
using DDDSample.Pathfinder;
using Infrastructure.Routing;
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
using NUnit.Framework;
using Environment=NHibernate.Cfg.Environment;

namespace Tests.Integration
{
   [TestFixture]
   public abstract class ScenarioTest
   {
      public static Location HONGKONG
      {
         get { return LocationRepository.Find(new UnLocode("CNHKG")); }
      }
      public static Location STOCKHOLM
      {
         get { return LocationRepository.Find(new UnLocode("SESTO")); }
      }
      public static Location TOKYO
      {
         get { return LocationRepository.Find(new UnLocode("JNTKO")); }
      }
      public static Location HAMBURG
      {
         get { return LocationRepository.Find(new UnLocode("DEHAM")); }
      }
      public static Location NEWYORK
      {
         get { return LocationRepository.Find(new UnLocode("USNYC")); }
      }
      public static Location CHICAGO
      {
         get { return LocationRepository.Find(new UnLocode("USCHI")); }
      }

      public static ICargoRepository CargoRepository
      {
         get { return ServiceLocator.Current.GetInstance<ICargoRepository>(); }
      }

      public static ILocationRepository LocationRepository
      {
         get { return ServiceLocator.Current.GetInstance<ILocationRepository>(); }
      }

      public static IBookingService BookingService
      {
         get { return ServiceLocator.Current.GetInstance<IBookingService>(); }
      }

      public static IHandlingEventService HandlingEventService
      {
         get { return ServiceLocator.Current.GetInstance<IHandlingEventService>(); }
      }
      
      private static IServiceLocator _ambientLocator;
      private static IUnityContainer _ambientContainer;
      private static ISessionFactory _sessionFactory;

      private string DatabaseFile;
      private ISession _currentSession;
         
      [SetUp]
      public void Initialize()
      {
         _ambientContainer = new UnityContainer();

         ConfigureNHibernateRepositories();

         _ambientContainer.RegisterType<IBookingService, BookingService>();
         _ambientContainer.RegisterType<IRoutingService, FakeRoutingService>();
         _ambientContainer.RegisterType<IHandlingEventService, HandlingEventService>();

         _ambientContainer.RegisterType<IEventHandler<CargoWasAssignedToRouteEvent>, CargoWasAssignedToRouteEventHandler>("cargoHasBeenAssignedToRouteEventHandler");
         _ambientContainer.RegisterType<IEventHandler<CargoWasHandledEvent>, CargoWasHandledEventHandler>("cargoWasHandledEventHandler");

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

      private static void ConfigureNHibernateRepositories()
      {
         _ambientContainer.RegisterType<ILocationRepository, DDDSample.Domain.Persistence.NHibernate.LocationRepository>();
         _ambientContainer.RegisterType<ICargoRepository, DDDSample.Domain.Persistence.NHibernate.CargoRepository>();

         _ambientContainer.AddNewExtension<Interception>();

         _ambientContainer.Configure<Interception>()

            .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
            .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())

            .AddPolicy("Transactions")
            .AddCallHandler<DDDSample.Domain.Persistence.NHibernate.TransactionCallHandler>()
            .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));         
      }
      
      private void InitializeNHibernate()
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
         
         _sessionFactory = cfg.BuildSessionFactory();
         _ambientContainer.RegisterInstance(_sessionFactory);         
         new SchemaExport(cfg).Execute(false, true, false);
         
         using (ISession session = _sessionFactory.OpenSession())
         {
            session.Save(new Location(new UnLocode("CNHKG"), "Hongkong"));
            session.Save(new Location(new UnLocode("AUMEL"), "Melbourne"));
            session.Save(new Location(new UnLocode("SESTO"), "Stockholm"));
            session.Save(new Location(new UnLocode("FIHEL"), "Helsinki"));
            session.Save(new Location(new UnLocode("USCHI"), "Chicago"));
            session.Save(new Location(new UnLocode("JNTKO"), "Tokyo"));
            session.Save(new Location(new UnLocode("DEHAM"), "Hamburg"));
            session.Save(new Location(new UnLocode("CNSHA"), "Shanghai"));
            session.Save(new Location(new UnLocode("NLRTM"), "Rotterdam"));
            session.Save(new Location(new UnLocode("SEGOT"), "Göteborg"));
            session.Save(new Location(new UnLocode("CNHGH"), "Hangzhou"));
            session.Save(new Location(new UnLocode("USNYC"), "New York"));
            session.Save(new Location(new UnLocode("USDAL"), "Dallas"));
            session.Flush();
         }

         _currentSession = _sessionFactory.OpenSession();
         
         NHibernate.Context.ThreadStaticSessionContext.Bind(_currentSession);
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
   }
}
