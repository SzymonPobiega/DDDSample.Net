using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Cargo.
   /// </summary>
   [Serializable]
   public class Cargo : AggregateRoot
   {
      private TrackingId _trackingId;
      private RouteSpecification _routeSpecification;
      private Itinerary _itinerary;
      private Delivery _deliveryStatus;

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

         Publish(this, new CargoRegisteredEvent(trackingId, routeSpecification,
                                        Delivery.DerivedFrom(_routeSpecification, _itinerary)));         
      }

      /// <summary>
      /// Specifies a new route for this cargo.
      /// </summary>
      /// <param name="destination">New destination.</param>
      public virtual void SpecifyNewRoute(UnLocode destination)
      {
         if (destination == null)
         {
            throw new ArgumentNullException("destination");
         }
         RouteSpecification routeSpecification = new RouteSpecification(_routeSpecification.Origin, destination,
                                                                        _routeSpecification.ArrivalDeadline);

         Publish(this, new CargoDestinationChangedEvent(routeSpecification,
                                                _deliveryStatus.Derive(routeSpecification, _itinerary)));         
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
         if (!_routeSpecification.IsSatisfiedBy(itinerary))
         {
            throw new InvalidOperationException("Provided itinerary doesn't satisfy this cargo's route specification.");
         }

         Publish(this, new CargoAssignedToRouteEvent(itinerary, _deliveryStatus.Derive(_routeSpecification, itinerary)));
      }      

      /// <summary>
      /// Registers new handling event into the history.
      /// </summary>
      /// <param name="eventType">Type of the event.</param>
      /// <param name="location">Location where event occured.</param>
      /// <param name="registrationDate">Date when event was registered.</param>
      /// <param name="completionDate">Date when action represented by the event was completed.</param>
      public virtual void RegisterHandlingEvent(HandlingEventType eventType, UnLocode location, DateTime registrationDate, DateTime completionDate)
      {
         HandlingEvent @event = new HandlingEvent(eventType, location, registrationDate, completionDate);
         
         Publish(this, new CargoHandledEvent(Delivery.DerivedFrom(_routeSpecification, _itinerary, @event)));
      }      

      private void OnCargoRegistered(CargoRegisteredEvent @event)
      {
         _trackingId = @event.TrackingId;
         _routeSpecification = @event.RouteSpecification;
         _deliveryStatus = @event.Delivery;
      }

      private void OnCargoAssignedToRoute(CargoAssignedToRouteEvent @event)
      {
         _itinerary = @event.NewItinerary;
         _deliveryStatus = @event.Delivery;
      }

      private void OnCargoDestinationChanged(CargoDestinationChangedEvent @event)
      {
         _routeSpecification = @event.NewSpecification;
         _deliveryStatus = @event.Delivery;
      }

      private void OnCargoHandled(CargoHandledEvent @event)
      {
         _deliveryStatus = @event.Delivery;
      }
      
      protected Cargo()
      {         
      }      
   }
}
