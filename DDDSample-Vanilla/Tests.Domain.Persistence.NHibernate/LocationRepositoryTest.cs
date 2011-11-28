using System.Collections.Generic;
using System.Linq;
using DDDSample.DomainModel.Persistence;
using DDDSample.DomainModel.Potential.Location;
using Domain.Persistence.Tests;
using NUnit.Framework;

namespace DDDSample.Domain.Persistence.Tests
{
   [TestFixture]
   public class LocationRepositoryTest : PersistenceTest
   {
      [Test]
      public void Find_OneSatisfyingItem_ResultReturned()
      {
         using (Scope(true))
         {
             Session.Save(new DomainModel.Potential.Location.Location(new UnLocode("PLKRK"), "Krakow"));
         }
         DomainModel.Potential.Location.Location location;
         using (Scope( true))
         {
            var repository = new LocationRepository(SessionFactory);
            location = repository.Find(new UnLocode("PLKRK"));
         }
         Assert.IsNotNull(location);
      }

      [Test]
      public void Find_NoSatisfyingItems_NullReturned()
      {
          DomainModel.Potential.Location.Location location;
         using (Scope(true))
         {
            var repository = new LocationRepository(SessionFactory);
            location = repository.Find(new UnLocode("PLKRK"));
         }
         Assert.IsNull(location);
      }

      [Test]
      [ExpectedException(typeof(NHibernate.NonUniqueResultException))]
      public void Find_ManySatisfyingItems_ExceptionThrown()
      {
         using (Scope(true))
         {
             Session.Save(new DomainModel.Potential.Location.Location(new UnLocode("PLKRK"), "Krakow"));
             Session.Save(new DomainModel.Potential.Location.Location(new UnLocode("PLKRK"), "Krakow2"));
         }         
         using (Scope(true))
         {
            var repository = new LocationRepository(SessionFactory);
            repository.Find(new UnLocode("PLKRK"));
         }         
      }


      [Test]
      public void FindAll_ResultsReturned()
      {
         using (Scope(true))
         {
             Session.Save(new DomainModel.Potential.Location.Location(new UnLocode("PLKRK"), "Krakow"));
             Session.Save(new DomainModel.Potential.Location.Location(new UnLocode("PLWAW"), "Warszawa"));
             Session.Save(new DomainModel.Potential.Location.Location(new UnLocode("PLWRC"), "Wroclaw"));
         }         
         using (Scope(true))
         {
            var repository = new LocationRepository(SessionFactory);
            IEnumerable<DomainModel.Potential.Location.Location> results = repository.FindAll();
            Assert.AreEqual(3, results.Count());
         }        
      }
   }
}
