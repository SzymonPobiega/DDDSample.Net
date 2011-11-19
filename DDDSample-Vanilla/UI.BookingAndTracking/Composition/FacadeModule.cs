using Autofac;
using DDDSample.UI.BookingAndTracking.Facade;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class FacadeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BookingServiceFacade>().AsSelf();
            builder.RegisterType<HandlingFacade>().AsSelf();
        }
    }
}