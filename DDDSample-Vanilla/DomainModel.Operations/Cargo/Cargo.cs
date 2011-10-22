using System;
using DDDSample.DomainModel.Potential.Customer;

namespace DDDSample.DomainModel.Operations.Cargo
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
      /// Gets delivery status of this cargo.
      /// </summary>
      public virtual Delivery Delivery { get; protected set; }

      /// <summary>
      /// Gets the ordering customer of this cargo.
      /// </summary>
      public virtual Customer OrderingCustomer { get; protected set; }

      /// <summary>
      /// Creates new <see cref="Cargo"/> object with provided tracking id and route specification.
      /// </summary>
      /// <param name="trackingId">Tracking id of this cargo.</param>
      /// <param name="routeSpecification">Route specification.</param>
      /// <param name="orderingCustomer">Customer who ordered this cargo.</param>
      public Cargo(TrackingId trackingId, RouteSpecification routeSpecification, Customer orderingCustomer)
      {
         if (trackingId == null)
         {
            throw new ArgumentNullException("trackingId");
         }
         if (routeSpecification == null)
         {
            throw new ArgumentNullException("routeSpecification");
         }
         OrderingCustomer = orderingCustomer;
         TrackingId = trackingId;
         RouteSpecification = routeSpecification;
         Delivery = Delivery.DerivedFrom(RouteSpecification, Itinerary, null);
      }

       /// <summary>
       /// Specifies a new route for this cargo.
       /// </summary>
       /// <param name="routeSpecification">Route specification.</param>
       /// <param name="eventPublisher"></param>
       public virtual void SpecifyNewRoute(RouteSpecification routeSpecification, IEventPublisher eventPublisher)
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
       /// <param name="eventPublisher"></param>
       public virtual void AssignToRoute(Itinerary itinerary, IEventPublisher eventPublisher)
      {
         if (itinerary == null)
         {
            throw new ArgumentNullException("itinerary");
         }
         var evnt = new CargoHasBeenAssignedToRouteEvent(this, Itinerary);
         Itinerary = itinerary;
         Delivery = Delivery.UpdateOnRouting(RouteSpecification, Itinerary);
         eventPublisher.Raise(evnt);
      }

       /// <summary>
       /// Updates delivery progress information according to handling history.
       /// </summary>
       /// <param name="lastHandlingEvent">Most recent handling event.</param>
       /// <param name="eventPublisher"></param>
       public virtual void DeriveDeliveryProgress(HandlingEvent lastHandlingEvent, IEventPublisher eventPublisher)
      {
         Delivery = Delivery.DerivedFrom(RouteSpecification, Itinerary, lastHandlingEvent);
         if (Delivery.IsMisdirected)
         {
            eventPublisher.Raise(new CargoWasMisdirectedEvent(this));
         }
         else if (Delivery.IsUnloadedAtDestination)
         {
            eventPublisher.Raise(new CargoHasArrivedEvent(this));
         }
      }
      
      protected Cargo()
      {         
      }      
   }
}