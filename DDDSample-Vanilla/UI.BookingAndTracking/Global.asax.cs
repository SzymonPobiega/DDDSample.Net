using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDSample.UI.BookingAndTracking.Composition;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;

namespace DDDSample.UI.BookingAndTracking
{
    public class MvcApplication : HttpApplication
    {
        private static NHibernateAmbientSessionManager _ambientSessionManager;

        public override void Init()
        {
            base.Init();
            WireUpSessionLifecycle();
        }

// ReSharper disable InconsistentNaming
        protected void Application_Start()
// ReSharper restore InconsistentNaming
        {
            RouteRegistrar.RegisterRoutes(RouteTable.Routes);
            ComposeApplication();
        }

        private static void ComposeApplication()
        {
            var container = new UnityContainer();
            container
                .AddNewExtension<RepositoryModule>()
                .AddNewExtension<NHibernateModule>()
                .AddNewExtension<ApplicationServicesModule>()
                .AddNewExtension<EventPublisherModule>();

            _ambientSessionManager = container.Resolve<NHibernateAmbientSessionManager>();
            var ambientLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => ambientLocator);
            ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory(container));
        }
        
        private void WireUpSessionLifecycle()
        {
            PreRequestHandlerExecute += (sender, e) => _ambientSessionManager.CreateAndBind();
            PostRequestHandlerExecute += (sender1, e1) => _ambientSessionManager.UnbindAndDispose();
        }
    }
}