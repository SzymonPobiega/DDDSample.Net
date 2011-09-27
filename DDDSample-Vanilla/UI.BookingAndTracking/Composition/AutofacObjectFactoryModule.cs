using Autofac;
using DDDSample.UI.BookingAndTracking.Infrastructure;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class AutofacObjectFactoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacObjectFactory>().AsImplementedInterfaces();
        }
    }
}