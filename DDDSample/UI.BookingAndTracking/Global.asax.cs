using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDSample.Application.SynchronousEventHandlers;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using DDDSample.Application;
using DDDSample.Application.Implemetation;
using Infrastructure.Routing;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace UI.BookingAndTracking
{
   // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
   // visit http://go.microsoft.com/?LinkId=9394801

   public class MvcApplication : HttpApplication
   {
      private static IServiceLocator _ambientLocator;
      private static IUnityContainer _ambientContainer;
      private static ISessionFactory _sessionFactory;

      public static void RegisterRoutes(RouteCollection routes)
      {
         _ambientContainer = new UnityContainer();                 

#if IN_MEMORY
         ConfigureInMemoryRepositories();
#elif NHIBERNATE
         ConfigureNHibernateRepositories();
#endif

         _ambientContainer.RegisterType<IBookingService, BookingService>();
         _ambientContainer.RegisterType<IRoutingService, RoutingService>();
         _ambientContainer.RegisterType<IHandlingEventService, HandlingEventService>();

         _ambientContainer.RegisterType<IEventHandler<CargoHasBeenAssignedToRouteEvent>, CargoHasBeenAssignedToRouteEventHandler>("cargoHasBeenAssignedToRouteEventHandler");
         _ambientContainer.RegisterType<IEventHandler<CargoWasHandledEvent>, CargoWasHandledEventHandler>("cargoWasHandledEventHandler");

         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(()=>_ambientLocator);

         ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory());

         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute(
             "Details",
             "Booking/CargoDetails/{trackingId}",
             new { controller = "Booking", action = "CargoDetails" }
         );

         routes.MapRoute(
             "Change destination",
             "Booking/ChangeDestination/{trackingId}",
             new { controller = "Booking", action = "ChangeDestination" }
         );

         routes.MapRoute(
             "Assign to route",
             "Booking/AssignToRoute/{trackingId}",
             new { controller = "Booking", action = "AssignToRoute" }
         );

         routes.MapRoute(
             "Default",                                              
             "{controller}/{action}/{id}",                           
             new { controller = "Home", action = "Index", id = "" }  
         );         

      }      

      protected void Application_Start()
      {         
         RegisterRoutes(RouteTable.Routes);         
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
         _ambientContainer.Configure<Interception>().SetInterceptorFor<IBookingService>(
            new InterfaceInterceptor())
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

      private static void InitializeNHibernate()
      {
         Configuration cfg = new Configuration().Configure();         

         _sessionFactory = cfg.BuildSessionFactory();
         _ambientContainer.RegisterInstance(_sessionFactory);                  
      }      

      public override void Init()
      {
         base.Init();         
#if NHIBERNATE
         PreRequestHandlerExecute += BindNHibernateSession;
         PostRequestHandlerExecute += UnbindNHibernateSession;         
#endif
      }      

      private static void BindNHibernateSession(object sender, EventArgs e)
      {
         NHibernate.Context.ManagedWebSessionContext.Bind(HttpContext.Current, _sessionFactory.OpenSession());
      }

      private static void UnbindNHibernateSession(object sender, EventArgs e)
      {
         NHibernate.Context.ManagedWebSessionContext.Unbind(HttpContext.Current, _sessionFactory).Dispose();
      }
   }
}