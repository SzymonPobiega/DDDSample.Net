using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
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

      public void Store(DomainModel.Operations.Cargo.Cargo cargo)
      {
         Session.Save(cargo);
      }

      public DomainModel.Operations.Cargo.Cargo Find(TrackingId trackingId)
      {         
         return Session.CreateCriteria(typeof(Cargo))
            .Add(Restrictions.Eq("TrackingId", trackingId))
            .UniqueResult<DomainModel.Operations.Cargo.Cargo>();
      }

      public IList<DomainModel.Operations.Cargo.Cargo> FindAll()
      {
         return Session.CreateCriteria(typeof(Cargo))
            .List<DomainModel.Operations.Cargo.Cargo>();
      }

      public TrackingId NextTrackingId()
      {
         Guid uniqueId = Guid.NewGuid();
         return new TrackingId(uniqueId.ToString("N"));
      }
   }
}