using System.Text;
using DDDSample.Application;
using DDDSample.Application.Implemetation;
using DDDSample.Application.SynchronousEventHandlers;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using DDDSample.Pathfinder;
using Infrastructure.Routing;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

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

#if IN_MEMORY
         ConfigureInMemoryRepositories();
#elif NHIBERNATE
         ConfigureNHibernateRepositories();
#endif

         _ambientContainer.RegisterType<IBookingService, BookingService>();
         _ambientContainer.RegisterType<IRoutingService, FakeRoutingService>();
         _ambientContainer.RegisterType<IHandlingEventService, HandlingEventService>();

         _ambientContainer.RegisterType<IEventHandler<CargoHasBeenAssignedToRouteEvent>, CargoHasBeenAssignedToRouteEventHandler>("cargoHasBeenAssignedToRouteEventHandler");
         _ambientContainer.RegisterType<IEventHandler<CargoWasHandledEvent>, CargoWasHandledEventHandler>("cargoWasHandledEventHandler");

         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(() => _ambientLocator);     
    
#if NHIBERNATE
         InitializeNHibernate();
#endif
      }

      private static void ConfigureNHibernateRepositories()
      {
         _ambientContainer.RegisterType<ILocationRepository, DDDSample.Domain.Persistence.NHibernate.LocationRepository>();
         _ambientContainer.RegisterType<ICargoRepository, DDDSample.Domain.Persistence.NHibernate.CargoRepository>();
         _ambientContainer.RegisterType<IHandlingEventRepository, DDDSample.Domain.Persistence.NHibernate.HandlingEventRepository>();

         _ambientContainer.AddNewExtension<Interception>();

         _ambientContainer.Configure<Interception>()

            .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
            .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())

            .AddPolicy("Transactions")
            .AddCallHandler<DDDSample.Domain.Persistence.NHibernate.TransactionCallHandler>()
            .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));         
      }

      private static void ConfigureInMemoryRepositories()
      {
         _ambientContainer.RegisterType<ILocationRepository, DDDSample.Domain.Persistence.InMemory.LocationRepository>();
         _ambientContainer.RegisterType<ICargoRepository, DDDSample.Domain.Persistence.InMemory.CargoRepository>();
         _ambientContainer.RegisterType<IHandlingEventRepository, DDDSample.Domain.Persistence.InMemory.HandlingEventRepository>();
      }

      private void InitializeNHibernate()
      {
         Configuration cfg = new Configuration().Configure();

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
   }
}
