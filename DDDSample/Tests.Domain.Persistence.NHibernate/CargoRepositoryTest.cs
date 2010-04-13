using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Persistence.NHibernate;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Potential.Location;
using NUnit.Framework;

namespace Domain.Persistence.Tests
{
   [TestFixture]
   public class CargoRepositoryTest : PersistenceTest
   {
      [Test]
      public void Store_EmptyStore_CargoSaved()
      {
         CargoRepository repository = new CargoRepository(SessionFactory);

         using (Scope(true))
         {
            Location krakow = new Location(new UnLocode("PLKRK"), "Krakow");
            Session.Save(krakow);

            Location warszawa = new Location(new UnLocode("PLWAW"), "Warszawa");
            Session.Save(warszawa);

            repository.Store(new Cargo(new TrackingId("xxx"),new RouteSpecification(krakow, warszawa, DateTime.Now), null));            
         }

         using (Scope(true))
         {
            IList<Cargo> cargos = Session.CreateQuery("from DDDSample.DomainModel.Operations.Cargo.Cargo c").List<Cargo>();
            Assert.AreEqual(1, cargos.Count);
         }
      }      
   }
}
