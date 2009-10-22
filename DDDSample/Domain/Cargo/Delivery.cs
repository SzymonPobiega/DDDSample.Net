using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Handling;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Description of delivery status.
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class Delivery : ValueObject
#pragma warning restore 661,660
   {
      private readonly TransportStatus _transportStatus;
      private readonly Location.Location _lastKnownLocation;      
      private readonly bool _misdirected;
      private readonly DateTime? _eta;
      //private HandlingActivity nextExpectedActivity;
      private readonly bool _isUnloadedAtDestination;
      private readonly RoutingStatus _routingStatus;
      private readonly DateTime _calculatedAt;
      private readonly HandlingHistory _handlingHistory;      

      /// <summary>
      /// Gets status of cargo routing.
      /// </summary>
      public RoutingStatus RoutingStatus
      {
         get { return _routingStatus; }
      }

      /// <summary>
      /// Gets time when this delivery status was calculated.
      /// </summary>
      public DateTime CalculatedAt
      {
         get { return _calculatedAt; }
      }

      /// <summary>
      /// Gets if this cargo has been unloaded at its destination.
      /// </summary>
      public bool IsUnloadedAtDestination
      {
         get { return _isUnloadedAtDestination; }
      }

      /// <summary>
      /// Gets estimated time of arrival. Returns null if information cannot be obtained (cargo is misrouted).
      /// </summary>
      public DateTime? EstimatedTimeOfArrival
      {
         get { return _eta; }
      }

      /// <summary>
      /// Gets last known location of this cargo.
      /// </summary>
      public Location.Location LastKnownLocation
      {
         get { return _lastKnownLocation; }
      }

      /// <summary>
      /// Gets status of cargo transport.
      /// </summary>
      public TransportStatus TransportStatus
      {
         get { return _transportStatus; }
      }

      /// <summary>
      /// Gets if this cargo was misdirected.
      /// </summary>
      public bool IsMisdirected
      {
         get { return _misdirected; }
      }

      /// <summary>
      /// Creates a new delivery snapshot based on the complete handling history of a cargo, as well 
      /// as its route specification and itinerary.
      /// </summary>
      /// <param name="specification">Current route specification.</param>
      /// <param name="itinerary">Current itinerary.</param>
      /// <param name="handlingHistory">Handling history.</param>
      /// <returns>Delivery status description.</returns>
      public static Delivery DerivedFrom(RouteSpecification specification, Itinerary itinerary, HandlingHistory handlingHistory)
      {         
         return new Delivery(handlingHistory, itinerary, specification);
      }

      /// <summary>
      /// Creates a new delivery snapshot to reflect changes in routing, i.e. when the route 
      /// specification or the itinerary has changed but no additional handling of the 
      /// cargo has been performed.
      /// </summary>
      /// <param name="routeSpecification">Current route specification.</param>
      /// <param name="itinerary">Current itinerary.</param>
      /// <returns>New delivery status description.</returns>
      public Delivery UpdateOnRouting(RouteSpecification routeSpecification, Itinerary itinerary)
      {
         if (routeSpecification == null)
         {
            throw new ArgumentNullException("routeSpecification");
         }         
         return new Delivery(_handlingHistory, itinerary, routeSpecification);
      }

      private Delivery(HandlingHistory handlingHistory, Itinerary itinerary, RouteSpecification specification)
      {
         _calculatedAt = DateTime.Now;
         _handlingHistory = handlingHistory;

         _misdirected = CalculateMisdirectionStatus(itinerary);
         _routingStatus = CalculateRoutingStatus(itinerary, specification);
         _transportStatus = CalculateTransportStatus();
         _lastKnownLocation = CalculateLastKnownLocation();         
         _eta = CalculateEta(itinerary);
         //this.nextExpectedActivity = calculateNextExpectedActivity(routeSpecification, itinerary);
         _isUnloadedAtDestination = CalculateUnloadedAtDestination(specification);
      }

      private bool CalculateUnloadedAtDestination(RouteSpecification specification)
      {
         return LastEvent != null && 
                  LastEvent.EventType == HandlingEventType.Unload &&
                  specification.Destination == LastEvent.Location;
      }

      private DateTime? CalculateEta(Itinerary itinerary)
      {
         return OnTrack ? itinerary.FinalArrivalDate : null;
      }      

      private Location.Location CalculateLastKnownLocation()
      {
         return LastEvent != null ? LastEvent.Location : null;
      }

      private TransportStatus CalculateTransportStatus()
      {
         if (LastEvent == null)
         {
            return TransportStatus.NotReceived;
         }

         switch (LastEvent.EventType)
         {
            case HandlingEventType.Load:
               return TransportStatus.OnboardCarrier;
            case HandlingEventType.Unload:
            case HandlingEventType.Receive:
            case HandlingEventType.Customs:
               return TransportStatus.InPort;
            case HandlingEventType.Claim:
               return TransportStatus.Claimed;
            default:
               return TransportStatus.Unknown;
         }
      }

      private static RoutingStatus CalculateRoutingStatus(Itinerary itinerary, RouteSpecification specification)
      {
         if (itinerary == null)
         {
            return RoutingStatus.NotRouted;
         }
         return specification.IsSatisfiedBy(itinerary) ? RoutingStatus.Routed : RoutingStatus.Misrouted;
      }

      private bool CalculateMisdirectionStatus(Itinerary itinerary)
      {
         if (LastEvent == null)
         {
            return false;
         }
         return !itinerary.IsExpected(LastEvent);
      }

      private bool OnTrack
      {
         get { return RoutingStatus == RoutingStatus.Routed && !IsMisdirected; }
      }

      private HandlingEvent LastEvent
      {
         get { return _handlingHistory != null ? _handlingHistory.EventsByCompletionTime.Last() : null; }
      }

      #region Infrastructure
      protected Delivery()
      {         
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _calculatedAt;
         yield return _eta;
         yield return _handlingHistory;
         yield return _isUnloadedAtDestination;
         yield return _isUnloadedAtDestination;
         yield return _lastKnownLocation;
         yield return _misdirected;
         yield return _routingStatus;
         yield return _transportStatus;
      }

      public static bool operator ==(Delivery left, Delivery right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(Delivery left, Delivery right)
      {
         return NotEqualOperator(left, right);
      }
      #endregion
   }
}
