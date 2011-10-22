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
        private readonly IEventPublisher _eventPublisher;

        public HandlingEventRepository(ISessionFactory sessionFactory, IEventPublisher eventPublisher)
            : base(sessionFactory)
        {
            _eventPublisher = eventPublisher;
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
            _eventPublisher.Raise(handlingEvent);
        }
    }
}