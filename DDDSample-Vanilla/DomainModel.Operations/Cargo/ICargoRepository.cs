using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.DomainModel.Operations.Cargo
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
      void Store(DomainModel.Operations.Cargo.Cargo cargo);
      /// <summary>
      /// Finds cargo by its trancking id.
      /// </summary>
      /// <param name="trackingId">Tracking id of a cargo.</param>
      /// <returns>Cargo instance if one present, null otherwise.</returns>
      DomainModel.Operations.Cargo.Cargo Find(TrackingId trackingId);
      /// <summary>
      /// Returns all cargos stored in the repository.
      /// </summary>
      /// <returns>A collection of all cargo objects.</returns>
      IList<DomainModel.Operations.Cargo.Cargo> FindAll();
      /// <summary>
      /// Returns next, unique tracking id value.
      /// </summary>
      /// <returns>New tracking id.</returns>
      TrackingId NextTrackingId();
   }
}