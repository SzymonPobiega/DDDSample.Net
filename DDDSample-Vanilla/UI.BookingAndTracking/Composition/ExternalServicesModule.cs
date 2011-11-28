using Autofac;
using DDDSample.DomainModel.DecisionSupport.Routing;
using DDDSample.Pathfinder;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class ExternalServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RoutingService>().AsImplementedInterfaces();
            builder.RegisterType<GraphTraversalService>().AsImplementedInterfaces();
        }
    }
}