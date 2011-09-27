using Autofac;
using DDDSample.Application.EventHandlers;
using DDDSample.UI.BookingAndTracking.Infrastructure;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class EventPublisherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacSimpleEventPublisher>().AsImplementedInterfaces();
            builder.RegisterType<CargoWasHandledEventHandler>().AsImplementedInterfaces();
        }
    }
}