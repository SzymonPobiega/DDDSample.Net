using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Potential.Voyage;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Voyage repository implementation based on NHibernate.
   /// </summary>
   public class VoyageRepository : AbstractRepository, IVoyageRepository
   {
      public VoyageRepository(ISessionFactory sessionFactory)
         : base(sessionFactory)
      {
      }      

      public IList<Voyage> FindBeginingBefore(DateTime deadline)
      {
         return Session.CreateCriteria(typeof (Voyage))
            .Add(Restrictions.Le("Schedule._departureTime", deadline))
            .List<Voyage>();
      }

      public Voyage Find(Guid voyageId)
      {
         return Session.Get<Voyage>(voyageId);
      }
   }
}