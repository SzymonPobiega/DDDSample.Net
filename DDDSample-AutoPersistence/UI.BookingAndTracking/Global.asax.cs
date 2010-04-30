using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDSample.Application;
using DDDSample.Application.AsynchronousEventHandlers.Messages;
using DDDSample.Application.Implemetation;
using DDDSample.Application.SynchronousEventHandlers;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Routing;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using NHibernate;
using NHibernate.ByteCode.LinFu;
using NHibernate.Cfg;
using NHibernate.Context;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.SagaPersisters.NHibernate;
using Synch = DDDSample.Application.SynchronousEventHandlers;
using Asynch = DDDSample.Application.AsynchronousEventHandlers;

namespace DDDSample.UI.BookingAndTracking
{
   // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
   // visit http://go.microsoft.com/?LinkId=9394801

   public class MvcApplication : HttpApplication
   {
      private static IUnityContainer _ambientContainer;
      private static IServiceLocator _ambientLocator;
      private static ISessionFactory _webSessionFactory;

      [Conditional("IN_MEMORY")]
      private static void ConfigureInMemory()
      {
         _ambientContainer = new UnityContainer();
         ConfigureInMemoryRepositories(_ambientContainer);
         ConfigureServices(_ambientContainer);
         ConfigureSynchEventHandlers(_ambientContainer);

         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(() => _ambientLocator);
         ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory());
      }

      [Conditional("NHIBERNATE")]
      private static void ConfigureNHibernateSynch()
      {
         _ambientContainer = new UnityContainer();
         ConfigureNHibernateRepositories(_ambientContainer);
         ConfigureServices(_ambientContainer);
         ConfigureSynchEventHandlers(_ambientContainer);
         ConfigureMVC();
         InitializeNHibernateForWeb(_ambientContainer);
      }

      [Conditional("NHIBERNATE_ASYNCH")]
      private static void ConfigureNHibernateAsynch()
      {
         _ambientContainer = new UnityContainer();
         IUnityContainer busContainer = new UnityContainer();

         ConfigureNHibernateRepositories(_ambientContainer);
         ConfigureNHibernateRepositories(busContainer);

         InitializeNHibernateForWeb(_ambientContainer);
         InitializeNHibernateForBus(busContainer);

         ConfigureServices(_ambientContainer);
         ConfigureServices(busContainer);

         IBus bus = InitializeBus(busContainer);
         _ambientContainer.RegisterInstance(bus);

         ConfigureAsynchEventHandlers(_ambientContainer);
         ConfigureMVC();
      }

      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute(
            "Details",
            "Booking/CargoDetails/{trackingId}",
            new {controller = "Booking", action = "CargoDetails"}
            );

         routes.MapRoute(
            "TrackingDetails",
            "Tracking/Track/{trackingId}",
            new {controller = "Tracking", action = "Track"}
            );

         routes.MapRoute(
            "Change destination",
            "Booking/ChangeDestination/{trackingId}",
            new {controller = "Booking", action = "ChangeDestination"}
            );

         routes.MapRoute(
            "Assign to route",
            "Booking/AssignToRoute/{trackingId}",
            new {controller = "Booking", action = "AssignToRoute"}
            );

         routes.MapRoute(
            "Default",
            "{controller}/{action}/{id}",
            new {controller = "Home", action = "Index", id = ""}
            );
      }

      private static void ConfigureMVC()
      {
         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(() => _ambientLocator);
         ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory());
      }

      private static void ConfigureSynchEventHandlers(IUnityContainer container)
      {
         container.RegisterType
            <IEventHandler<CargoHasBeenAssignedToRouteEvent>, CargoHasBeenAssignedToRouteEventHandler>(
            "cargoHasBeenAssignedToRouteEventHandler");
         container.RegisterType<IEventHandler<CargoWasHandledEvent>, CargoWasHandledEventHandler>(
            "cargoWasHandledEventHandler");
      }

      private static void ConfigureAsynchEventHandlers(IUnityContainer container)
      {
         container.RegisterType
            <IEventHandler<CargoHasBeenAssignedToRouteEvent>,
               Application.AsynchronousEventHandlers.EventHandlers.CargoHasBeenAssignedToRouteEventHandler>(
            "cargoHasBeenAssignedToRouteEventHandler");
         container.RegisterType
            <IEventHandler<CargoWasHandledEvent>,
               Application.AsynchronousEventHandlers.EventHandlers.CargoWasHandledEventHandler>(
            "cargoWasHandledEventHandler");
      }

      private static void ConfigureServices(IUnityContainer container)
      {
         container.RegisterType<IBookingService, BookingService>();
         container.RegisterType<IRoutingService, RoutingService>();
         container.RegisterType<IHandlingEventService, HandlingEventService>();
      }

      protected void Application_Start()
      {
         RegisterRoutes(RouteTable.Routes);

         ConfigureInMemory();
         ConfigureNHibernateSynch();
         ConfigureNHibernateAsynch();
      }

      private static IBus InitializeBus(IUnityContainer container)
      {
         InitializeNHibernateForBus(container);
         IBus bus = Configure.WithWeb()
            .UnityBuilder(container)
            .XmlSerializer()
            .MsmqTransport()
            .IsTransactional(true)
            .PurgeOnStartup(false)
            .UnicastBus()
            .ImpersonateSender(false)
            .LoadMessageHandlers()
            .DBSubcriptionStorage()                            
            .CreateBus()
            .Start();
         bus.Subscribe<CargoHasBeenAssignedToRouteMessage>();
         bus.Subscribe<CargoWasHandledMessage>();
         return bus;
      }

      private static void ConfigureInMemoryRepositories(IUnityContainer container)
      {
         container.RegisterType<ILocationRepository, LocationRepository>();
         container.RegisterType<ICargoRepository, CargoRepository>();
         container.RegisterType<IHandlingEventRepository, HandlingEventRepository>();
      }

      private static void ConfigureNHibernateRepositories(IUnityContainer container)
      {
         container.RegisterType<ILocationRepository, Domain.Persistence.NHibernate.LocationRepository>();
         container.RegisterType<ICargoRepository, Domain.Persistence.NHibernate.CargoRepository>();
         container.RegisterType<IHandlingEventRepository, Domain.Persistence.NHibernate.HandlingEventRepository>();

         container.AddNewExtension<Interception>();
         container.Configure<Interception>()
            .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
            .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())
            .AddPolicy("Transactions")
            .AddCallHandler<TransactionCallHandler>()
            .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));
      }

      private static void InitializeNHibernateForWeb(IUnityContainer container)
      {
         //Configuration cfg = new Configuration().Configure();
         //cfg.AddProperties(new Dictionary<string, string>
         //                     {
         //                        {"current_session_context_class", "NHibernate.Context.WebSessionContext"}
         //                     });
         Configuration cfg = InitializeNHibernate(x => x.CurrentSessionContext<WebSessionContext>());
         _webSessionFactory = cfg.BuildSessionFactory();
         container.RegisterInstance(_webSessionFactory);
      }

      public static Configuration InitializeNHibernate(Func<SQLiteConfiguration, SQLiteConfiguration> configCallback)
      {
         return Fluently.Configure()
            .Database(configCallback(SQLiteConfiguration.Standard
                         .ProxyFactoryFactory<ProxyFactoryFactory>()                         
                         .ShowSql()
                         .UsingFile(@"|DataDirectory|\dddsample.sqlite")))
            .Mappings(m => m.AutoMappings
                              .Add(AutoMap.AssemblyOf<Location>()
                                      .Where(t => typeof(IEntity).IsAssignableFrom(t))
                                      .Setup(s =>
                                                {
                                                   s.IsComponentType =
                                                      type => typeof(ValueObject).IsAssignableFrom(type);
                                                   s.GetComponentColumnPrefix =
                                                      property => property.PropertyType.Name + "_";
                                                })
                                      .Conventions.AddFromAssemblyOf<IntraAggregateEntityCollectionConvention>()
                              ))
            .ExposeConfiguration(x => x.AddAssembly("DDDSample.Domain.Persistence.NHibernate"))
            .BuildConfiguration();
      }

      private static void InitializeNHibernateForBus(IUnityContainer container)
      {
         Configuration cfg = InitializeNHibernate(x => x.CurrentSessionContext<ThreadStaticSessionContext>());
         ISessionFactory factory = cfg.BuildSessionFactory();
         container.RegisterInstance(factory);
      }

      public override void Init()
      {
         base.Init();
         WireUpSessionLifecycle();
      }

      private void WireUpSessionLifecycle()
      {
         PreRequestHandlerExecute += BindNHibernateSession;
         PostRequestHandlerExecute += UnbindNHibernateSession;
      }
      
      private static void BindNHibernateSession(object sender, EventArgs e)
      {
         CurrentSessionContext.Bind(_webSessionFactory.OpenSession());
      }
      
      private static void UnbindNHibernateSession(object sender, EventArgs e)
      {
         ISession session = CurrentSessionContext.Unbind(_webSessionFactory);
         if (session!= null)
         {
            session.Dispose();
         }
      }
   }

   public static class NHibernateMessageModuleConfig
   {
      public static Configure NHibernateMessageModule(this Configure configure)
      {
         configure.Configurer.ConfigureComponent<NHibernateMessageModule>(ComponentCallModelEnum.Singlecall);
         return configure;
      }
   }
}