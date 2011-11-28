using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.DomainModel.Persistence
{
    /// <summary>
    /// Cargo repository implementation based on NHibernate.
    /// </summary>
    public class CargoRepository : AbstractRepository, ICargoRepository
    {
        public CargoRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        #region ICargoRepository Members

        public void Store(Cargo cargo)
        {
            Session.Save(cargo);
        }

        public Cargo Find(TrackingId trackingId)
        {
            return Session.CreateCriteria<Cargo>()
                .Add(Restrictions.Eq("TrackingId", trackingId))
                .UniqueResult<Cargo>();
        }

        public IList<Cargo> FindAll()
        {
            return Session.CreateCriteria(typeof (Cargo))
                .List<Cargo>();
        }

        public TrackingId NextTrackingId()
        {
            Guid uniqueId = Guid.NewGuid();
            return new TrackingId(uniqueId.ToString("N"));
        }

        #endregion
    }
}