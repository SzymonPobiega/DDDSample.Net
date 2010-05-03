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
         const string query = @"from DDDSample.Domain.Handling.HandlingEvent e where e.Cargo.TrackingId = :trackingId";
         var events = Session.CreateQuery(query).SetString("trackingId", cargoTrackingId.IdString)
            .List<HandlingEvent>();
         return new HandlingHistory(events);
      }

      public void Store(HandlingEvent handlingEvent)
      {
         Session.Save(handlingEvent);
      }

      public HandlingEvent Find(Guid uniqueId)
      {
         return Session.Get<HandlingEvent>(uniqueId);
      }

      public void Store(HandlingHistory handlingHistory)
      {
         Session.Save(handlingHistory);
      }
   }
}