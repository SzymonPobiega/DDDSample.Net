using System;
using System.Collections.Generic;
using System.Linq;
using DDDSample.Domain.Location;
using NHibernate;
using Raven.Client;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Location repository implementation based on NHibernate.
   /// </summary>
   public class LocationRepository : ILocationRepository
   {
      private readonly IDocumentStore _documentStore;

      public LocationRepository(IDocumentStore documentStore)
      {
         _documentStore = documentStore;         
      }
     
      public IList<Location.Location> FindAll()
      {
         using (var session = _documentStore.OpenSession())
         {
            return session.Query<Location.Location>().ToList();
         }
      }
   }
}