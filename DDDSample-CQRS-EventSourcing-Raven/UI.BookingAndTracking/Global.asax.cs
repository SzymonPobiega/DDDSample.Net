using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDSample.CommandHandlers;
using DDDSample.Commands;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.EventHandlers;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Raven.Client.Document;

namespace DDDSample.UI.BookingAndTracking
{   
   public class MvcApplication : HttpApplication
   {
      private static IUnityContainer _ambientContainer;
      private static IServiceLocator _ambientLocator;

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

      protected void Application_Start()
      {
         log4net.Config.XmlConfigurator.Configure();
         RegisterRoutes(RouteTable.Routes);
         Configure();
      }

      private static void Configure()
      {
         _ambientContainer = new UnityContainer();

         ConfigureRepositories(_ambientContainer);
         ConfigureDocumentStore(_ambientContainer);
         ConfigureBus(_ambientContainer);
         ConfigureEventDenormalizers(_ambientContainer);
         ConfigureMVC();
      }

      private static void ConfigureRepositories(IUnityContainer container)
      {
         container.RegisterType<ILocationRepository, LocationRepository>();
         container.RegisterType<ICargoRepository, CargoRepository>();
      }

      private static void ConfigureDocumentStore(IUnityContainer container)
      {
         var resolver = new PropertiesOnlyContractResolver
         {
            DefaultMembersSearchFlags =
               BindingFlags.Public | BindingFlags.NonPublic |
               BindingFlags.Instance
         };
         var documentStore = new DocumentStore
         {
            Url = "http://localhost:8080",
            Conventions = new DocumentConvention
            {
               JsonContractResolver = resolver
            }
         }.Initialise();
         container.RegisterInstance(documentStore);
      }


      private static void ConfigureBus(IUnityContainer container)
      {
         container.RegisterType<IBus, InProcessBus>();
         container.RegisterType<ICommandHandler<AssignCargoToRouteCommand>,
            AssignCargoToRouteCommandHandler>();
         container.RegisterType<ICommandHandler<BookNewCargoCommand>,
            BookNewCargoCommandHandler>();
         container.RegisterType<ICommandHandler<ChangeDestinationCommand>,
            ChangeDestinationCommandHandler>();
         container.RegisterType<ICommandHandler<RegisterHandlingEventCommand>,
            RegisterHandlingEventCommandHandler>();
      }

      private static void ConfigureEventDenormalizers(IUnityContainer container)
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

      private static void ConfigureMVC()
      {
         _ambientLocator = new UnityServiceLocator(_ambientContainer);
         ServiceLocator.SetLocatorProvider(() => _ambientLocator);
         ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory());
      }       
   }
}