using System;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using Microsoft.Practices.Unity;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class RepositoryModule : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<ILocationRepository, LocationRepository>();
            Container.RegisterType<ICargoRepository, CargoRepository>();
            Container.RegisterType<IHandlingEventRepository, HandlingEventRepository>();
        }
    }
}