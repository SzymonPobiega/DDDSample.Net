using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Handling;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Cargo.
   /// </summary>
   public class Cargo
   {      
      /// <summary>
      /// Gets the tracking id of this cargo.
      /// </summary>
      public virtual TrackingId TrackingId { get; protected set; }

      /// <summary>
      /// Gets the route specification of this cargo.
      /// </summary>
      public virtual RouteSpecification RouteSpecification { get; protected set; }

      /// <summary>
      /// Gets the itinerary of this cargo.
      /// </summary>
      public virtual Itinerary Itinerary { get; protected set; }

      /// <summary>
      /// Creates new <see cref="Cargo"/> object with provided tracking id and route specification.
      /// </summary>
      /// <param name="trackingId">Tracking id of this cargo.</param>
      /// <param name="routeSpecification">Route specification.</param>
      public Cargo(TrackingId trackingId, RouteSpecification routeSpecification)
      {
         TrackingId = trackingId;
         RouteSpecification = routeSpecification;
      }

      /// <summary>
      /// Specifies a new route for this cargo.
      /// </summary>
      /// <param name="routeSpecification">Route specification.</param>
      public virtual void SpecifyNewRoute(RouteSpecification routeSpecification)
      {
         RouteSpecification = routeSpecification;
      }

      /// <summary>
      /// Assigns cargo to a provided route.
      /// </summary>
      /// <param name="itinerary"></param>
      public virtual void AssignToRoute(Itinerary itinerary)
      {
         CargoHasBeenAssignedToRouteEvent @event = new CargoHasBeenAssignedToRouteEvent(this, Itinerary);
         Itinerary = itinerary;
         DomainEvents.Raise(@event);
      }

      /// <summary>
      /// Updates delivery progress information according to handling history.
      /// </summary>
      /// <param name="handlingHistory">A collection of handling events associated with this cargo.</param>
      public virtual void DeriveDeliveryProgress(HandlingHistory handlingHistory)
      {
      }
      
      protected Cargo()
      {         
      }      
   }
}
