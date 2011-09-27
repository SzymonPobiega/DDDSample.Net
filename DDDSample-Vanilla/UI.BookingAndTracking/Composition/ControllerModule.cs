using Autofac;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class ControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof (ControllerModule).Assembly)
                .Where(x => x.Name.EndsWith("Controller"))
                .AsSelf();
        } 
    }
}