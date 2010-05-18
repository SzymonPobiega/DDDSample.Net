using System;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using NHibernate;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Cargo repository implementation based on NHibernate.
   /// </summary>
   public class CargoRepository : ICargoRepository
   {      
      public void Store(Cargo.Cargo cargo)
      {
         UnitOfWork.Current.Track(cargo);
      }

      public Cargo.Cargo Find(string id)
      {
         return (Cargo.Cargo)UnitOfWork.Current.LoadById(id);
      }      

      public TrackingId NextTrackingId()
      {
         Guid uniqueId = Guid.NewGuid();
         return new TrackingId(uniqueId.ToString("N"));
      }
   }
}