using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DDDSample.Domain.Handling;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Cargo.
   /// </summary>
   public class Cargo : IAggregateRoot<Cargo>
   {
      public virtual Guid Id { get; protected set; }
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
      /// Gets delivery status of this cargo.
      /// </summary>
      public virtual Delivery Delivery { get; protected set; }

      /// <summary>
      /// Creates new <see cref="Cargo"/> object with provided tracking id and route specification.
      /// </summary>
      /// <param name="trackingId">Tracking id of this cargo.</param>
      /// <param name="routeSpecification">Route specification.</param>
      public Cargo(TrackingId trackingId, RouteSpecification routeSpecification)
      {
         if (trackingId == null)
         {
            throw new ArgumentNullException("trackingId");
         }
         if (routeSpecification == null)
         {
            throw new ArgumentNullException("routeSpecification");
         }
         TrackingId = trackingId;
         RouteSpecification = routeSpecification;
         Delivery = Delivery.DerivedFrom(RouteSpecification, Itinerary, null);
      }

      /// <summary>
      /// Specifies a new route for this cargo.
      /// </summary>
      /// <param name="routeSpecification">Route specification.</param>
      public virtual void SpecifyNewRoute(RouteSpecification routeSpecification)
      {
         if (routeSpecification == null)
         {
            throw new ArgumentNullException("routeSpecification");
         }
         RouteSpecification = routeSpecification;
         Delivery = Delivery.UpdateOnRouting(RouteSpecification, Itinerary);
      }

      /// <summary>
      /// Assigns cargo to a provided route.
      /// </summary>
      /// <param name="itinerary">New itinerary</param>
      public virtual void AssignToRoute(Itinerary itinerary)
      {
         if (itinerary == null)
         {
            throw new ArgumentNullException("itinerary");
         }
         CargoHasBeenAssignedToRouteEvent @event = new CargoHasBeenAssignedToRouteEvent(this, Itinerary);
         Itinerary = itinerary;
         Delivery = Delivery.UpdateOnRouting(RouteSpecification, Itinerary);
         DomainEvents.Raise(@event);
      }

      /// <summary>
      /// Updates delivery progress information according to handling history.
      /// </summary>
      /// <param name="lastHandlingEvent">Most recent handling event.</param>
      public virtual void DeriveDeliveryProgress(HandlingEvent lastHandlingEvent)
      {
         Delivery = Delivery.DerivedFrom(RouteSpecification, Itinerary, lastHandlingEvent);
         if (Delivery.IsMisdirected)
         {
            DomainEvents.Raise(new CargoWasMisdirectedEvent(this));
         }
         else if (Delivery.IsUnloadedAtDestination)
         {
            DomainEvents.Raise(new CargoHasArrivedEvent(this));
         }
      }
      
      protected Cargo()
      {         
      }      
   }
}
