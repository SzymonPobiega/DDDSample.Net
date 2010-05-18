using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Provides access to cargo store.
   /// </summary>
   public interface ICargoRepository
   {
      /// <summary>
      /// Persistes given cargo instance.
      /// </summary>
      /// <param name="cargo">New cargo instance.</param>
      void Store(Cargo cargo);
      /// <summary>
      /// Finds cargo by its trancking id.
      /// </summary>
      /// <param name="id">Primary key of a cargo.</param>
      /// <returns>Cargo instance if one present, null otherwise.</returns>
      Cargo Find(string id);      
      /// <summary>
      /// Returns next, unique tracking id value.
      /// </summary>
      /// <returns>New tracking id.</returns>
      TrackingId NextTrackingId();
   }
}
