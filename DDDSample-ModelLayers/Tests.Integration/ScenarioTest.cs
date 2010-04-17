using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DDDSample.Application.EventHandlers;
using DDDSample.Application.Services;
using DDDSample.Domain;
using DDDSample.Domain.Location;
using DDDSample.DomainModel;
using DDDSample.DomainModel.DecisionSupport.Routing;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Persistence;
using DDDSample.DomainModel.Policy.Commitments;
using DDDSample.DomainModel.Policy.Routing;
using DDDSample.DomainModel.Potential.Customer;
using DDDSample.DomainModel.Potential.Location;
using DDDSample.DomainModel.Potential.Voyage;
using DDDSample.Pathfinder;
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
      public static ICargoRepository CargoRepository
      {
         get { return ServiceLocator.Current.GetInstance<ICargoRepository>(); }
      }

      public static ILocationRepository LocationRepository
      {
         get { return ServiceLocator.Current.GetInstance<ILocationRepository>(); }
      }

      public static IHandlingEventRepository HandlingEventRepository
      {
         get { return ServiceLocator.Current.GetInstance<IHandlingEventRepository>(); }
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
      
      private ISession _currentSession;
         
      [SetUp]
      public void Initialize()
      {
         _ambientContainer = new UnityContainer();

         ConfigureNHibernateRepositories();

         _ambientContainer.RegisterType<IBookingService, BookingService>();
         _ambientContainer.RegisterType<IRoutingService, RoutingService>();
         _ambientContainer.RegisterType<IGraphTraversalService, GraphTraversalService>();
         _ambientContainer.RegisterType<IHandlingEventService, HandlingEventService>();

         _ambientContainer.RegisterType<IEventHandler<CargoHasBeenAssignedToRouteEvent>, CargoHasBeenAssignedToRouteEventHandler>("cargoHasBeenAssignedToRouteEventHandler");
         _ambientContainer.RegisterType<IEventHandler<CargoWasHandledEvent>, CargoWasHandledEventHandler>("cargoWasHandledEventHandler");

         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(() => _ambientLocator);     
    
         InitializeNHibernate();
      }

      [TearDown]
      public void TearDownTests()
      {
         _sessionFactory.Dispose();         
      }

      private static void ConfigureNHibernateRepositories()
      {
         _ambientContainer.RegisterType<ILocationRepository, DDDSample.Domain.Persistence.NHibernate.LocationRepository>();
         _ambientContainer.RegisterType<IVoyageRepository, DDDSample.Domain.Persistence.NHibernate.VoyageRepository>();
         _ambientContainer.RegisterType<ICargoRepository, DDDSample.Domain.Persistence.NHibernate.CargoRepository>();
         _ambientContainer.RegisterType<IHandlingEventRepository, DDDSample.Domain.Persistence.NHibernate.HandlingEventRepository>();
         _ambientContainer.RegisterType<ICustomerRepository, DDDSample.Domain.Persistence.NHibernate.CustomerRepository>();
         _ambientContainer.RegisterType<ICustomerAgreementRepository, DDDSample.Domain.Persistence.NHibernate.CustomerAgreementRepository>();

         _ambientContainer.AddNewExtension<Interception>();

         _ambientContainer.Configure<Interception>()

            .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
            .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())

            .AddPolicy("Transactions")
            .AddCallHandler<LocalTransactionCallHandler>()
            .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));         
      }

      private void InitializeNHibernate()
      {         
         var cfg = new Configuration();
            cfg.AddProperties(new Dictionary<string, string>
                              {
                                 {Environment.ConnectionDriver, typeof (SQLite20Driver).FullName},
                                 {Environment.Dialect, typeof (SQLiteDialect).FullName},
                                 {Environment.ConnectionProvider, typeof (DriverConnectionProvider).FullName},
                                 {Environment.ConnectionString, "Data Source=:memory:;Version=3;New=True;"},
                                 {
                                    Environment.ProxyFactoryFactoryClass,
                                    typeof (ProxyFactoryFactory).AssemblyQualifiedName
                                    },
                                 {
                                    Environment.CurrentSessionContextClass,
                                    typeof (ThreadStaticSessionContext).AssemblyQualifiedName
                                    },
                                 {Environment.ReleaseConnections,"on_close"},
                                 {Environment.Hbm2ddlAuto, "create"},
                                 {Environment.ShowSql, true.ToString()}
                              });
         cfg.AddAssembly("DDDSample.DomainModel.Persistence");

         _sessionFactory = cfg.BuildSessionFactory();
         _ambientContainer.RegisterInstance(_sessionFactory);         

         ISession session = _sessionFactory.OpenSession();

         new SchemaExport(cfg).Execute(false, true, false, session.Connection, Console.Out);

         SampleLocations.CreateLocations(session);
         SampleTransportLegs.CreateTransportLegs(session);
         SampleVoyages.CreateVoyages(session);
         SampleCustomers.CreateCustomers(session);
         session.Flush();

         _currentSession = session;

         CurrentSessionContext.Bind(_currentSession);
      }
   }
}
