using System;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using NHibernate;
using HandlingEvent = DDDSample.DomainModel.Operations.Handling.HandlingEvent;

namespace DDDSample.DomainModel.Persistence
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
   }
}