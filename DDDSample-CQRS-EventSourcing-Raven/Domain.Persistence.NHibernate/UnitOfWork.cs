using System;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using Raven.Client;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class UnitOfWork
   {      
      private readonly List<IAggregateRoot> _trackedObjects = new List<IAggregateRoot>();
      private readonly IDocumentStore _documentStore;      

      public UnitOfWork(IDocumentStore documentStore)
      {
         _documentStore = documentStore;         
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
         using (var session = _documentStore.OpenSession())
         {            
            DoCommit(session);              
            session.SaveChanges();
         }         
      }

      public void Rollback()
      {         
      }

      private void DoCommit(IDocumentSession session)
      {
         foreach (IAggregateRoot root in _trackedObjects)
         {
            Store(session, root);
         }
      }

      public IAggregateRoot LoadById(string id)
      {
         IList<Event> events;
         using (var session = _documentStore.OpenSession())
         {
            var metadada = session.Load<AggregateRootMetadata>(MakeRootId(id));
            events = session.Query<Event>()
               .WaitForNonStaleResults()
               .Where(x => x.Version >= metadada.RecentSnapshotVersion)
               .Where(x => x.EntityId == id)
               .OrderBy(x => x.Version)               
               .ToList();
         }                
         var lastSnapshot = (IAggregateRoot)events[0].Data;
         lastSnapshot.Id = id;
         lastSnapshot.LoadFromEventStream(events.Skip(1).Select(x => x.Data));
         lastSnapshot.OriginalVersion = events.Last().Version;

         _trackedObjects.Add(lastSnapshot);

         return lastSnapshot;
      }

      private static string MakeRootId(string id)
      {
         return "root/"+id;
      }

      public void Store(IDocumentSession session, IAggregateRoot root)
      {
         if (ShouldPersistSnapshot(root))
         {
            StoreSnapshot(session, root);
            SetRecentSnaphotVersion(session, root);
         }
         else
         {
            StoreEvents(session, root);
         }
      }

      private static void StoreSnapshot(IDocumentSession session, IAggregateRoot root)
      {
         root.CurrentVersion = root.OriginalVersion+1;
         var instance = new Event
                           {
                              EntityId = root.Id,
                              Version = root.CurrentVersion,
                              IsSnapshot = true,
                              Data = root
                           };        
         session.Store(instance);         
      }

      private static void SetRecentSnaphotVersion(IDocumentSession session, IAggregateRoot root)
      {
         var metadata = session.Load<AggregateRootMetadata>(root.Id);
         if (metadata == null)
         {
            metadata = new AggregateRootMetadata
                          {
                             Id = MakeRootId(root.Id),
                             RecentSnapshotVersion = root.CurrentVersion
                          };            
            session.Store(metadata);
         }
         else
         {
            metadata.RecentSnapshotVersion = root.CurrentVersion;
         }         
      }

      private static void StoreEvents(IDocumentSession session, IAggregateRoot root)
      {
         var version = root.OriginalVersion;
         foreach (var @event in root.Events)
         {
            version++;
            var instance = new Event
                                {
                                   EntityId = root.Id,
                                   Version = version,
                                   IsSnapshot = false,
                                   Data = @event
                                };
            session.Store(instance);
         }
         root.OriginalVersion = version;
         root.Events.Clear();
      }

      private static bool ShouldPersistSnapshot(IAggregateRoot root)
      {
         return root.OriginalVersion % 3 == 0;
      }
   }
}