using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace DDDSample.UI.BookingAndTracking
{
    /// <summary>
    /// A controller factory based on abient DI container.
    /// </summary>
    public class ContainerControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public ContainerControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController) _container.Resolve(controllerType);
        }
    }
}
