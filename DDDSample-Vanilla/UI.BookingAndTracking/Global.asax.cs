using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using DDDSample.UI.BookingAndTracking.Composition;
using DDDSample.UI.BookingAndTracking.Infrastructure;

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
            IContainer container = null;
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<ApplicationServicesModule>();
            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<NHibernateModule>();
            containerBuilder.RegisterModule<EventPublisherModule>();
            containerBuilder.RegisterModule<AutofacObjectFactoryModule>();
            containerBuilder.RegisterModule<CommandFilterModule>();
            containerBuilder.RegisterModule<ControllerModule>();
            containerBuilder.RegisterModule<FacadeModule>();
            containerBuilder.RegisterModule<DTOAssemblerModule>();
            containerBuilder.RegisterModule<ExternalServicesModule>();

            containerBuilder.Register(x => container);

            container = containerBuilder.Build();

            _ambientSessionManager = container.Resolve<NHibernateAmbientSessionManager>();
            ControllerBuilder.Current.SetControllerFactory(new ContainerControllerFactory(container));
        }
        
        private void WireUpSessionLifecycle()
        {
            PreRequestHandlerExecute += (sender, e) => _ambientSessionManager.CreateAndBind();
            PostRequestHandlerExecute += (sender1, e1) => _ambientSessionManager.UnbindAndDispose();
        }
    }
}