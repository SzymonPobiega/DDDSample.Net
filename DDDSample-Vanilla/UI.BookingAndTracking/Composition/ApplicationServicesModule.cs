using System;
using Autofac;
using DDDSample.Application.CommandHandlers;
using LeanCommandUnframework;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class ApplicationServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var handlerTypes = new[]
                                   {
                                       typeof (AssignCargoToRouteCommandHandler),
                                       typeof (BookNewCargoCommandHandler),
                                       typeof (ChangeDestinationCommandHandler),
                                       typeof (RegisterHandlingEventCommandHandler),
                                       typeof (RequestPossibleRoutesForCargoCommandHandler)
                                   };

            builder.RegisterInstance(new HandlerSelector(handlerTypes)).AsSelf();
            builder.Register(CreatePipelineFactory).SingleInstance().AsSelf();
            foreach (var handlerType in handlerTypes)
            {
                builder.RegisterType(handlerType).AsSelf();
            }
        }

        private static PipelineFactory CreatePipelineFactory(IComponentContext context)
        {
            return new PipelineFactory(context.Resolve<HandlerSelector>(), context.Resolve<FilterSelector>(),
                                       context.Resolve<IObjectFactory>());
        }
    }
}