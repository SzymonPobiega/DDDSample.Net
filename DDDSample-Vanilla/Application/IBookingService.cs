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
      /// <returns>Cargo tracing id for referencing this cargo.</returns>
      TrackingId BookNewCargo(UnLocode origin, UnLocode destination, DateTime arrivalDeadline);
      /// <summary>
      /// Requests a list of itineraries describing possible routes for this cargo.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <returns>A list of possible itineraries for this cargo.</returns>
      IList<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId);
      /// <summary>
      /// Assigns cargo identified by <paramref name="trackingId"/> to a new route.
      /// </summary>
      /// <param name="itinerary">Itinerary describing the selected route.</param>
      /// <param name="trackingId">cargo tracking id.</param>
      void AssignCargoToRoute(TrackingId trackingId, Itinerary itinerary);
      /// <summary>
      /// Changes the destination of a cargo.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <param name="unLocode">UN locode of new destination.</param>
      void ChangeDestination(TrackingId trackingId, UnLocode unLocode);
   }
}
