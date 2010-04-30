using System;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using NHibernate;

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
         const string query = @"from DDDSample.Domain.Handling.HandlingHistory h where h.TrackingId = :trackingId";
         return Session.CreateQuery(query).SetString("trackingId", cargoTrackingId.IdString)
            .UniqueResult<HandlingHistory>();
      }

      public void Store(HandlingHistory handlingHistory)
      {
         Session.Save(handlingHistory);
      }
   }
}