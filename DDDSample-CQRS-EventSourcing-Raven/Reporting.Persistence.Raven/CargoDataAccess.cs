using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Reporting;
using Raven.Client;

namespace Reporting.Persistence.Raven
{
   public class CargoDataAccess
   {
      private readonly IDocumentStore _documentStore;

      public CargoDataAccess(IDocumentStore documentStore)
      {
         _documentStore = documentStore;
      }

      public void Store(Cargo cargo)
      {
         using (var session = _documentStore.OpenSession())
         {
            session.Store(cargo);  
            session.SaveChanges();
         }
      }

      public Cargo FindByTrackingId(string trackingId)
      {
         using (var session = _documentStore.OpenSession())
         {
            return session.Query<Cargo>("CargoByTrackingId")
               .Where(string.Format("TrackingId:{0}",trackingId))
               .SingleOrDefault();
         }
      }

      public IList<Cargo> FindAll()
      {
         using (var session = _documentStore.OpenSession())
         {
            return session.Query<Cargo>().ToList();
         }
      }
   }
}
