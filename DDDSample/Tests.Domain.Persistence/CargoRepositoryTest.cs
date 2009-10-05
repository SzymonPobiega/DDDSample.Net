using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Persistence.Tests
{
   [TestClass]
   public class CargoRepositoryTest : PersistenceTest
   {
      [TestMethod]
      public void Store_EmptyStore_CargoSaved()
      {
         CargoRepository repository = new CargoRepository(SessionFactory);

         using (Scope(true))
         {
            Location krakow = new Location(new UnLocode("PLKRK"), "Krakow");
            Session.Save(krakow);

            Location warszawa = new Location(new UnLocode("PLWAW"), "Warszawa");
            Session.Save(warszawa);

            repository.Store(new Cargo(new TrackingId("xxx"),new RouteSpecification(krakow, warszawa, DateTime.Now) ));            
         }

         using (Scope(true))
         {
            IList<Cargo> cargoes = Session.CreateQuery("from DDDSample.Domain.Cargo.Cargo c").List<Cargo>();
            Assert.AreEqual(1, cargoes.Count);
         }
      }      
   }
}
