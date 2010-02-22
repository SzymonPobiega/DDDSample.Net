using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

      private RouteSpecification _routeSpecification;
      private Itinerary _itinerary;
      private HandlingEvent _lastHandlingEvent;
      private readonly IList<HandlingEvent> _handlingEvents;

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
         _handlingEvents = new List<HandlingEvent>();

         TrackingId = trackingId;
         _routeSpecification = routeSpecification;
         Delivery delivery = Delivery.DerivedFrom(_routeSpecification, _itinerary, _lastHandlingEvent);         
         DomainEvents.Raise(new CargoRegisteredEvent(this, _routeSpecification, delivery));
      }

      /// <summary>
      /// Specifies a new route for this cargo.
      /// </summary>
      /// <param name="destination">New destination.</param>
      public virtual void SpecifyNewRoute(Location.Location destination)
      {
         if (destination == null)
         {
            throw new ArgumentNullException("destination");
         }
         RouteSpecification routeSpecification = new RouteSpecification(_routeSpecification.Origin, destination, _routeSpecification.ArrivalDeadline);

         Delivery delivery = Delivery.DerivedFrom(routeSpecification, _itinerary, _lastHandlingEvent);
         CargoDestinationChangedEvent @event = new CargoDestinationChangedEvent(this, routeSpecification, _routeSpecification, delivery);         
         _routeSpecification = routeSpecification;
         DomainEvents.Raise(@event);
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
         Delivery delivery = Delivery.DerivedFrom(_routeSpecification, itinerary, _lastHandlingEvent);
         CargoWasAssignedToRouteEvent @event = new CargoWasAssignedToRouteEvent(this, _itinerary, itinerary, delivery);
         _itinerary = itinerary;         
         DomainEvents.Raise(@event);
      }      

      /// <summary>
      /// Registers new handling event into the history.
      /// </summary>
      /// <param name="eventType">Type of the event.</param>
      /// <param name="location">Location where event occured.</param>
      /// <param name="registrationDate">Date when event was registered.</param>
      /// <param name="completionDate">Date when action represented by the event was completed.</param>
      public virtual void RegisterHandlingEvent(HandlingEventType eventType, Location.Location location, DateTime registrationDate, DateTime completionDate)
      {
         HandlingEvent @event = new HandlingEvent(eventType, location, registrationDate, completionDate, this);
         _handlingEvents.Add(@event);
         _lastHandlingEvent = @event;
         Delivery currentDelivery = Delivery.DerivedFrom(_routeSpecification, _itinerary, @event);         
         DomainEvents.Raise(new CargoWasHandledEvent(this, currentDelivery, eventType));
      }

      /// <summary>
      /// Returns a collection of routes which comply to this cargo's route specification.
      /// </summary>
      /// <param name="routingService">Routing service to use for searching for possible routes.</param>
      /// <returns></returns>
      public virtual IList<Itinerary> RequestPossibleRoutes(IRoutingService routingService)
      {
         return routingService.FetchRoutesForSpecification(_routeSpecification);
      }
      
      protected Cargo()
      {         
      }      
   }
}
