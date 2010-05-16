using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace DDDSample.Reporting.Persistence.NHibernate
{
   public class CargoDataAccess
   {
      private readonly ISessionFactory _sessionFactory;

      public CargoDataAccess(ISessionFactory sessionFactory)
      {
         _sessionFactory = sessionFactory;
      }

      public void Store(Cargo cargo)
      {
         _sessionFactory.GetCurrentSession().Save(cargo);
      }

      public Cargo Find(string trackingId)
      {
         const string query = @"from DDDSample.Reporting.Cargo c where c.TrackingId = :trackingId";
         return _sessionFactory.GetCurrentSession().CreateQuery(query).SetString("trackingId", trackingId)
            .UniqueResult<Cargo>();
      }

      public Cargo Find(Guid cargoId)
      {
         return _sessionFactory.GetCurrentSession().Get<Cargo>(cargoId);
      }

      public IList<Cargo> FindAll()
      {
         const string query = @"from DDDSample.Reporting.Cargo c";
         return _sessionFactory.GetCurrentSession().CreateQuery(query)
            .List<Cargo>();
      }
   }
}
