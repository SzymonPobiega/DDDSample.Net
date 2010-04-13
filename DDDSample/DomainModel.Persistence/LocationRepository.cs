using System;
using System.Collections.Generic;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Potential.Location;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Location repository implementation based on NHibernate.
   /// </summary>
   public class LocationRepository : AbstractRepository, ILocationRepository
   {
      public LocationRepository(ISessionFactory sessionFactory) : base(sessionFactory)
      {
      }

      public DomainModel.Potential.Location.Location Find(UnLocode locode)
      {         
         return Session.CreateCriteria(typeof(DomainModel.Potential.Location.Location))
            .Add(Restrictions.Eq("UnLocode", locode))
            .UniqueResult<DomainModel.Potential.Location.Location>();
      }

      public IList<DomainModel.Potential.Location.Location> FindAll()
      {         
         return Session.CreateCriteria(typeof(DomainModel.Potential.Location.Location))
            .List<DomainModel.Potential.Location.Location>();
      }
   }
}