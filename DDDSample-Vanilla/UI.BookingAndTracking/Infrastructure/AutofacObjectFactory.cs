using System;
using Autofac;
using LeanCommandUnframework;

namespace DDDSample.UI.BookingAndTracking.Infrastructure
{
    public class AutofacObjectFactory : IObjectFactory
    {
        private readonly IContainer _container;

        public AutofacObjectFactory(IContainer container)
        {
            _container = container;
        }

        public object GetHandlerInstance(Type handlerType)
        {
            return _container.Resolve(handlerType);
        }

        public object GetFilterInstance(Type filterType)
        {
            return _container.Resolve(filterType);
        }
    }
}