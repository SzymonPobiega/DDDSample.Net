using Autofac;
using DDDSample.Application.Assemblers;
using DDDSample.UI.BookingAndTracking.Facade;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class DTOAssemblerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LegDTOAssembler>().AsSelf();
            builder.RegisterType<RouteCandidateDTOAssember>().AsSelf();
            builder.RegisterType<CargoRoutingDTOAssembler>().AsSelf();
        }
    }
}