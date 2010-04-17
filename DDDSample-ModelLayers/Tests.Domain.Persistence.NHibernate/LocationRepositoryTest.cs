using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Persistence.NHibernate;
using DDDSample.DomainModel.Potential.Location;
using NUnit.Framework;

namespace Domain.Persistence.Tests
{
   [TestFixture]
   public class LocationRepositoryTest : PersistenceTest
   {
      [Test]
      public void Find_OneSatisfyingItem_ResultReturned()
      {
         using (Scope(true))
         {
            Session.Save(new Location(new UnLocode("PLKRK"), "Krakow"));
         }
         Location location;
         using (Scope( true))
         {
            LocationRepository repository = new LocationRepository(SessionFactory);
            location = repository.Find(new UnLocode("PLKRK"));
         }
         Assert.IsNotNull(location);
      }

      [Test]
      public void Find_NoSatisfyingItems_NullReturned()
      {         
         Location location;
         using (Scope(true))
         {
            LocationRepository repository = new LocationRepository(SessionFactory);
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
            Session.Save(new Location(new UnLocode("PLKRK"), "Krakow"));
            Session.Save(new Location(new UnLocode("PLKRK"), "Krakow2"));
         }         
         using (Scope(true))
         {
            LocationRepository repository = new LocationRepository(SessionFactory);
            repository.Find(new UnLocode("PLKRK"));
         }         
      }


      [Test]
      public void FindAll_ResultsReturned()
      {
         using (Scope(true))
         {
            Session.Save(new Location(new UnLocode("PLKRK"), "Krakow"));
            Session.Save(new Location(new UnLocode("PLWAW"), "Warszawa"));
            Session.Save(new Location(new UnLocode("PLWRC"), "Wroclaw"));
         }         
         using (Scope(true))
         {
            LocationRepository repository = new LocationRepository(SessionFactory);
            IEnumerable<Location> results = repository.FindAll();
            Assert.AreEqual(3, results.Count());
         }        
      }
   }
}
