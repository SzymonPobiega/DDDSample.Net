using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class UnitOfWork
   {      
      private readonly List<IAggregateRoot> _trackedObjects = new List<IAggregateRoot>();
      private readonly ISessionFactory _sessionFactory;
      private readonly IStatelessSession _readSession;

      public UnitOfWork(ISessionFactory sessionFactory)
      {
         _sessionFactory = sessionFactory;
         _readSession = _sessionFactory.OpenStatelessSession();
      }

      [ThreadStatic]
      private static UnitOfWork _current;

      public static UnitOfWork Current
      {
         get { return _current; }
         set { _current = value; }
      }

      public void Track(IAggregateRoot root)
      {         
         _trackedObjects.Add(root);
      }

      public void Commit()
      {
         using (ISession session = _sessionFactory.OpenSession())
         {
            if (Transaction.Current == null)
            {
               session.BeginTransaction();
            }
            DoCommit(session);
            if (Transaction.Current == null)
            {
               session.Transaction.Commit();
               session.Transaction.Dispose();
            }
         }
         _readSession.Dispose();
      }

      public void Rollback()
      {
         _readSession.Dispose();
      }

      private void DoCommit(ISession session)
      {
         foreach (IAggregateRoot root in _trackedObjects)
         {
            Store(session, root);
         }
      }

      public IAggregateRoot LoadById(Guid id)
      {
         DetachedCriteria boundaryCriteria = DetachedCriteria.For<Event>()
            .SetProjection(Projections.Id())
            .Add(Expression.Eq("IsSnapshot", true))
            .Add(Expression.Eq("EntityId", id))
            .AddOrder(new Order("SequenceNumber", false))
            .SetFirstResult(0)
            .SetMaxResults(1);         

         ICriteria criteria = _readSession.CreateCriteria(typeof(Event))
            .Add(Expression.Eq("EntityId", id))
            .Add(Subqueries.PropertyGe("SequenceNumber", boundaryCriteria))
            .AddOrder(new Order("SequenceNumber", true));

         IList<Event> events = criteria.List<Event>();
         if (events.Count < 1)
         {
            throw new ArgumentException(string.Format("Entity with id {0} not found.", id));
         }
         IAggregateRoot lastSnapshot = (IAggregateRoot)events[0].Data;
         lastSnapshot.Id = id;
         lastSnapshot.LoadFromEventStream(events.Skip(1).Select(x => x.Data));
         lastSnapshot.Version = events.Last().Version;

         _trackedObjects.Add(lastSnapshot);

         return lastSnapshot;
      }

      public void Store(ISession session, IAggregateRoot root)
      {         
         StoreEvents(session, root);
         if (ShouldPersistSnapshot(root))
         {
            StoreSnapshot(session, root);
         }
      }

      private static void StoreSnapshot(ISession session, IAggregateRoot root)
      {
         Event instance = new Event
                             {
                                EntityId = root.Id,
                                Version = root.Version,
                                IsSnapshot = true,
                                Data = root
                             };
         session.Save(instance);
      }

      private static void StoreEvents(ISession session, IAggregateRoot root)
      {
         int version = root.Version;
         foreach (object @event in root.Events)
         {
            version++;
            Event instance = new Event
                                {
                                   EntityId = root.Id,
                                   Version = version,
                                   IsSnapshot = false,
                                   Data = @event
                                };
            session.Save(instance);
         }
         root.Version = version;
         root.Events.Clear();
      }

      private static bool ShouldPersistSnapshot(IAggregateRoot root)
      {
         return root.Version % 3 == 1;
      }
   }
}