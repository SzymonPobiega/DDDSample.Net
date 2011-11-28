using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Persistence;
using DDDSample.DomainModel.Potential.Location;
using Domain.Persistence.Tests;
using NUnit.Framework;

namespace DDDSample.Domain.Persistence.Tests
{
   [TestFixture]
   public class CargoRepositoryTest : PersistenceTest
   {
      [Test]
      public void Store_EmptyStore_CargoSaved()
      {
         var repository = new CargoRepository(SessionFactory);

         using (Scope(true))
         {
            var krakow = new DomainModel.Potential.Location.Location(new UnLocode("PLKRK"), "Krakow");
            Session.Save(krakow);

            var warszawa = new DomainModel.Potential.Location.Location(new UnLocode("PLWAW"), "Warszawa");
            Session.Save(warszawa);

            repository.Store(new Cargo(new TrackingId("xxx"),new RouteSpecification(krakow, warszawa, DateTime.Now), null));            
         }

         using (Scope(true))
         {
            var cargos = Session.CreateCriteria<Cargo>().List<Cargo>();
            Assert.AreEqual(1, cargos.Count);
         }
      }      
   }
}
