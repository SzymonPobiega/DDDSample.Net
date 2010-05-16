using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;

namespace DDDSample.Domain.Persistence.InMemory
{
   public class CargoRepository : ICargoRepository
   {      
      private static readonly List<Cargo.Cargo> _storage =  new List<Cargo.Cargo>(); 

      public void Store(Cargo.Cargo cargo)
      {
         _storage.Add(cargo);
      }

      public Cargo.Cargo Find(TrackingId trackingId)
      {
         return _storage.FirstOrDefault(x => x.TrackingId == trackingId);
      }

      public IList<Cargo.Cargo> FindAll()
      {
         return new List<Cargo.Cargo>(_storage);
      }

      public TrackingId NextTrackingId()
      {
         return new TrackingId(Guid.NewGuid().ToString("N"));
      }
   }
}
