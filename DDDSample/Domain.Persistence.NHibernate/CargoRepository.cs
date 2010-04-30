using System;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using NHibernate;

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

      public void Store(Cargo.Cargo cargo)
      {
         Session.Save(cargo);
      }

      public Cargo.Cargo Find(TrackingId trackingId)
      {
         const string query = @"from DDDSample.Domain.Cargo.Cargo c where c.TrackingId = :trackingId";
         return Session.CreateQuery(query).SetString("trackingId", trackingId.IdString)
            .UniqueResult<Cargo.Cargo>();
      }

      public IList<Cargo.Cargo> FindAll()
      {
         const string query = @"from DDDSample.Domain.Cargo.Cargo c";
         return Session.CreateQuery(query)
            .List<Cargo.Cargo>();
      }

      public TrackingId NextTrackingId()
      {
         Guid uniqueId = Guid.NewGuid();
         return new TrackingId(uniqueId.ToString("N"));
      }
   }
}