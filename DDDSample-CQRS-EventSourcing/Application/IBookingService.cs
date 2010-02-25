using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;
using DDDSample.Domain.Cargo;

namespace DDDSample.Application
{
   /// <summary>
   /// Cargo booking service.
   /// </summary>
   public interface IBookingService
   {
      /// <summary>
      /// Registers a new cargo in the tracking system, not yet routed.
      /// </summary>
      /// <param name="origin">Cargo origin UN locode.</param>
      /// <param name="destination">Cargo destination UN locode.</param>
      /// <param name="arrivalDeadline">Arrival deadline.</param>
      /// <param name="id"></param>
      /// <returns>Cargo tracing id for referencing this cargo.</returns>
      Guid BookNewCargo(UnLocode origin, UnLocode destination, DateTime arrivalDeadline, out TrackingId id);
      /// <summary>
      /// Requests a list of itineraries describing possible routes for this cargo.
      /// </summary>
      /// <param name="id">Cargo tracking id.</param>
      /// <returns>A list of possible itineraries for this cargo.</returns>
      IList<Itinerary> RequestPossibleRoutesForCargo(Guid id);
      /// <summary>
      /// Assigns cargo identified by <paramref name="id"/> to a new route.
      /// </summary>
      /// <param name="itinerary">Itinerary describing the selected route.</param>
      /// <param name="id">cargo tracking id.</param>
      void AssignCargoToRoute(Guid id, Itinerary itinerary);
      /// <summary>
      /// Changes the destination of a cargo.
      /// </summary>
      /// <param name="id">Cargo tracking id.</param>
      /// <param name="unLocode">UN locode of new destination.</param>
      void ChangeDestination(Guid id, UnLocode unLocode);
   }
}
