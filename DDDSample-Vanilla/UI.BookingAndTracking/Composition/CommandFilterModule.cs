using System;
using Autofac;
using DDDSample.UI.BookingAndTracking.Infrastructure;
using LeanCommandUnframework;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class CommandFilterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var filterTypes = new[] {typeof (NHibernateTransactionCommandFilter)};

            builder.RegisterInstance(CreateFilterSelector(filterTypes));

            foreach (var filterType in filterTypes)
            {
                builder.RegisterType(filterType).AsSelf();
            }
        }

        private static FilterSelector CreateFilterSelector(Type[] filterTypes)
        {
            return new FilterSelector(filterTypes);
        }
    }
}