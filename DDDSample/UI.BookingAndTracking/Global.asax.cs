using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence;
using DDDSampleNET.Application;
using DDDSampleNET.Application.Implemetation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
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

         _ambientContainer.RegisterType<ILocationRepository, LocationRepository>();
         _ambientContainer.RegisterType<ICargoRepository, CargoRepository>();
         _ambientContainer.RegisterType<IBookingService, BookingService>();

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
             "Default",                                              // Route name
             "{controller}/{action}/{id}",                           // URL with parameters
             new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
         );         

      }      

      protected void Application_Start()
      {         
         RegisterRoutes(RouteTable.Routes);         
         InitializeNHibernate();
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
         PreRequestHandlerExecute += BindNHibernateSession;
         PostRequestHandlerExecute += UnbindNHibernateSession;         
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