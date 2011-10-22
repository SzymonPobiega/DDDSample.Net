using System.Collections.Generic;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Potential.Location;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.DomainModel.Persistence
{
   /// <summary>
   /// Location repository implementation based on NHibernate.
   /// </summary>
   public class LocationRepository : AbstractRepository, ILocationRepository
   {
      public LocationRepository(ISessionFactory sessionFactory) : base(sessionFactory)
      {
      }

      public Location Find(UnLocode locode)
      {         
         return Session.CreateCriteria(typeof(Location))
            .Add(Restrictions.Eq("UnLocode", locode))
            .UniqueResult<Location>();
      }

      public IList<Location> FindAll()
      {         
         return Session.CreateCriteria(typeof(Location))
            .List<Location>();
      }
   }
}