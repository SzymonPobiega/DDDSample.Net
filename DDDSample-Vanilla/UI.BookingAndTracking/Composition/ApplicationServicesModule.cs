using System;
using DDDSample.Application;
using DDDSample.Application.Implemetation;
using DDDSample.Domain;
using DDDSample.Domain.Persistence.NHibernate;
using Infrastructure.Routing;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class ApplicationServicesModule : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IBookingService, BookingService>();
            Container.RegisterType<IRoutingService, RoutingService>();
            Container.RegisterType<IHandlingEventService, HandlingEventService>();

            Container.AddNewExtension<Interception>();
            Container.Configure<Interception>()
                .SetInterceptorFor<IBookingService>(new InterfaceInterceptor())
                .SetInterceptorFor<IHandlingEventService>(new InterfaceInterceptor())
                .AddPolicy("Transactions")
                .AddCallHandler<TransactionCallHandler>()
                .AddMatchingRule(new AssemblyMatchingRule("DDDSample.Application"));
        }
    }
}