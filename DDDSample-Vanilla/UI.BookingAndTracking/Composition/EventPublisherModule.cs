using System;
using DDDSample.Application;
using DDDSample.Application.SynchronousEventHandlers;
using DDDSample.Domain;
using DDDSample.Domain.Handling;
using Microsoft.Practices.Unity;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class EventPublisherModule : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IEventPublisher, SimpleEventPublisher>();
            Container.RegisterType<IEventHandler<CargoWasHandledEvent>, CargoWasHandledEventHandler>(
               "cargoWasHandledEventHandler");
        }
    }
}