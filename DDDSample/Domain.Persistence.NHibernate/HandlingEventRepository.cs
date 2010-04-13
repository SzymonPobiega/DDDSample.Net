using System;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Handling event repository implementation based on NHibernate.
   /// </summary>
   public class HandlingEventRepository : AbstractRepository, IHandlingEventRepository
   {
      public HandlingEventRepository(ISessionFactory sessionFactory)
         : base(sessionFactory)
      {
      }
      
      public HandlingHistory LookupHandlingHistoryOfCargo(TrackingId cargoTrackingId)
      {         
         return Session.CreateCriteria(typeof(HandlingHistory))
            .Add(Restrictions.Eq("TrackingId", cargoTrackingId))            
            .UniqueResult<HandlingHistory>();
      }

      public void Store(HandlingHistory handlingHistory)
      {
         Session.Save(handlingHistory);
      }
   }
}