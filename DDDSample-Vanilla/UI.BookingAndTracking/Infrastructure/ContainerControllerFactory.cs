using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace DDDSample.UI.BookingAndTracking.Infrastructure
{
    /// <summary>
    /// A controller factory based on abient DI container.
    /// </summary>
    public class ContainerControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public ContainerControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController) _container.Resolve(controllerType);
        }
    }
}
