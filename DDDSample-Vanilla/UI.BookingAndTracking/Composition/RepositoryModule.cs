using Autofac;
using DDDSample.Domain.Persistence.NHibernate;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof (LocationRepository).Assembly)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
        }
    }
}