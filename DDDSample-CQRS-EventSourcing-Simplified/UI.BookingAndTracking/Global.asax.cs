using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.EventHandlers;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace DDDSample.UI.BookingAndTracking
{   
   public class MvcApplication : HttpApplication
   {
      private static IUnityContainer _ambientContainer;
      private static IServiceLocator _ambientLocator;
      private static ISessionFactory _webSessionFactory;
            
      private static void ConfigureNHibernate()
      {
         _ambientContainer = new UnityContainer();
         ConfigureNHibernateRepositories(_ambientContainer);
         InitializeNHibernateForWeb(_ambientContainer);
         ConfigureEventHandlers(_ambientContainer);
         ConfigureMVC();
      }

      private static void ConfigureEventHandlers(IUnityContainer container)
      {
         container.RegisterType
            <IEventHandler<Cargo, CargoAssignedToRouteEvent>,
               CargoWasAssignedToRouteEventHandler>(
            "cargoHasBeenAssignedToRouteEventHandler");
         container.RegisterType
            <IEventHandler<Cargo, CargoHandledEvent>,
               CargoWasHandledEventHandler>(
            "cargoWasHandledEventHandler");
         container.RegisterType
            <IEventHandler<Cargo, CargoRegisteredEvent>,
               CargoRegisteredEventHandler>(
            "cargoRegisteredEventHandler");
         container.RegisterType
            <IEventHandler<Cargo, CargoDestinationChangedEvent>,
               CargoDestinationChangedEventHandler>(
            "cargoDestinationChangedEventHandler");
      }

      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         routes.IgnoreRoute("favicon.ico");

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
            "Booking/ChangeDestination/{TrackingId}",
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

      protected void Application_Start()
      {
         log4net.Config.XmlConfigurator.Configure();
         RegisterRoutes(RouteTable.Routes);
         ConfigureNHibernate();
      }        
      
      private static void ConfigureNHibernateRepositories(IUnityContainer container)
      {
         container.RegisterType<ILocationRepository, LocationRepository>();
         container.RegisterType<ICargoRepository, CargoRepository>();

         //container.AddNewExtension<Interception>();
         //container.Configure<Interception>()
         //   .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
         //   .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())
         //   .AddPolicy("Transactions")
         //   .AddCallHandler<TransactionCallHandler>()
         //   .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));
      }

      private static void InitializeNHibernateForWeb(IUnityContainer container)
      {
         Configuration cfg = new Configuration().Configure();
         cfg.AddProperties(new Dictionary<string, string>
                              {
                                 {"current_session_context_class", "NHibernate.Context.WebSessionContext"}
                              });
         _webSessionFactory = cfg.BuildSessionFactory();
         container.RegisterInstance(_webSessionFactory);
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
         CurrentSessionContext.Unbind(_webSessionFactory).Dispose();
      }
   }   
}