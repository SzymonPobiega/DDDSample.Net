using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
      private readonly bool _isUnloadedAtDestination;
      private readonly RoutingStatus _routingStatus;
      private readonly DateTime _calculatedAt;
      private readonly HandlingEvent _lastEvent;
      private readonly HandlingActivity _nextExpectedActivity;


      /// <summary>
      /// Gets next expected activity.
      /// </summary>
      public HandlingActivity NextExpectedActivity
      {
         get { return _nextExpectedActivity; }
      }

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
      /// <param name="lastHandlingEvent">Most recent handling event.</param>
      /// <returns>Delivery status description.</returns>
      public static Delivery DerivedFrom(RouteSpecification specification, Itinerary itinerary, HandlingEvent lastHandlingEvent)
      {
         return new Delivery(lastHandlingEvent, itinerary, specification);
      }      

      private Delivery(HandlingEvent lastHandlingEvent, Itinerary itinerary, RouteSpecification specification)
      {
         _calculatedAt = DateTime.Now;
         _lastEvent = lastHandlingEvent;

         _misdirected = CalculateMisdirectionStatus(itinerary);
         _routingStatus = CalculateRoutingStatus(itinerary, specification);
         _transportStatus = CalculateTransportStatus();
         _lastKnownLocation = CalculateLastKnownLocation();
         _eta = CalculateEta(itinerary);
         _nextExpectedActivity = CalculateNextExpectedActivity(specification, itinerary);
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

      private HandlingActivity CalculateNextExpectedActivity(RouteSpecification routeSpecification, Itinerary itinerary)
      {
         if (!OnTrack)
         {
            return null;
         }

         if (LastEvent == null)
         {
            return new HandlingActivity(HandlingEventType.Receive, routeSpecification.Origin);
         }

         switch (LastEvent.EventType)
         {
            case HandlingEventType.Load:

               Leg lastLeg = itinerary.Legs.FirstOrDefault(x => x.LoadLocation == LastEvent.Location);
               return lastLeg != null ? new HandlingActivity(HandlingEventType.Unload, lastLeg.UnloadLocation) : null;

            case HandlingEventType.Unload:
               IEnumerator<Leg> enumerator = itinerary.Legs.GetEnumerator();
               while (enumerator.MoveNext())
               {
                  if (enumerator.Current.UnloadLocation == LastEvent.Location)
                  {
                     Leg currentLeg = enumerator.Current;
                     return enumerator.MoveNext() ? new HandlingActivity(HandlingEventType.Load, enumerator.Current.LoadLocation) : new HandlingActivity(HandlingEventType.Claim, currentLeg.UnloadLocation);
                  }
               }
               return null;

            case HandlingEventType.Receive:
               Leg firstLeg = itinerary.Legs.First();
               return new HandlingActivity(HandlingEventType.Load, firstLeg.LoadLocation);
            default:
               return null;
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
         get { return _lastEvent; }
      }

      #region Infrastructure
      protected Delivery()
      {
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _calculatedAt;
         yield return _eta;
         yield return _lastEvent;
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
