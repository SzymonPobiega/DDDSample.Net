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
      public TrackingId TrackingId { get; private set; }
      public RouteSpecification RouteSpecification { get; private set; }
      public Itinerary Itinerary { get; private set; }
      public Delivery DeliveryStatus { get; private set; }

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
                                        Delivery.DerivedFrom(RouteSpecification, Itinerary)));         
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
         var routeSpecification = new RouteSpecification(RouteSpecification.Origin, destination,
                                                                        RouteSpecification.ArrivalDeadline);

         Publish(this, new CargoDestinationChangedEvent(routeSpecification,
                                                DeliveryStatus.Derive(routeSpecification, Itinerary)));         
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
         if (!RouteSpecification.IsSatisfiedBy(itinerary))
         {
            throw new InvalidOperationException("Provided itinerary doesn't satisfy this cargo's route specification.");
         }

         Publish(this, new CargoAssignedToRouteEvent(itinerary, DeliveryStatus.Derive(RouteSpecification, itinerary)));
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
         var @event = new HandlingEvent(eventType, location, registrationDate, completionDate);
         
         Publish(this, new CargoHandledEvent(Delivery.DerivedFrom(RouteSpecification, Itinerary, @event)));
      }      

      private void OnCargoRegistered(CargoRegisteredEvent @event)
      {
         TrackingId = @event.TrackingId;
         RouteSpecification = @event.RouteSpecification;
         DeliveryStatus = @event.Delivery;
      }

      private void OnCargoAssignedToRoute(CargoAssignedToRouteEvent @event)
      {
         Itinerary = @event.NewItinerary;
         DeliveryStatus = @event.Delivery;
      }

      private void OnCargoDestinationChanged(CargoDestinationChangedEvent @event)
      {
         RouteSpecification = @event.NewSpecification;
         DeliveryStatus = @event.Delivery;
      }

      private void OnCargoHandled(CargoHandledEvent @event)
      {
         DeliveryStatus = @event.Delivery;
      }
      
      protected Cargo()
      {         
      }      
   }
}
