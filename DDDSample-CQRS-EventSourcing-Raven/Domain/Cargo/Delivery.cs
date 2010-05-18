using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{   
   /// <summary>
   /// Description of delivery status.
   /// </summary>
   [Serializable]
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class Delivery : ValueObject
#pragma warning restore 661,660
   {
      public HandlingEvent LastEvent { get; private set; }

      /// <summary>
      /// Gets next expected activity.
      /// </summary>
      public HandlingActivity NextExpectedActivity { get; private set; }

      /// <summary>
      /// Gets status of cargo routing.
      /// </summary>
      public RoutingStatus RoutingStatus { get; private set; }

      /// <summary>
      /// Gets time when this delivery status was calculated.
      /// </summary>
      public DateTime CalculatedAt { get; private set; }

      /// <summary>
      /// Gets if this cargo has been unloaded at its destination.
      /// </summary>
      public bool IsUnloadedAtDestination { get; private set; }

      /// <summary>
      /// Gets estimated time of arrival. Returns null if information cannot be obtained (cargo is misrouted).
      /// </summary>
      public DateTime? EstimatedTimeOfArrival { get; private set; }

      /// <summary>
      /// Gets last known location of this cargo.
      /// </summary>
      public UnLocode LastKnownLocation { get; private set; }

      /// <summary>
      /// Gets status of cargo transport.
      /// </summary>
      public TransportStatus TransportStatus { get; private set; }

      /// <summary>
      /// Gets if this cargo was misdirected.
      /// </summary>
      public bool IsMisdirected { get; private set; }

      /// <summary>
      /// Gets type of last recorded event.
      /// </summary>
      public HandlingEventType? LastEventType
      {
         get { return LastEvent != null ? LastEvent.EventType : (HandlingEventType?)null; }
      }

      /// <summary>
      /// Creates a new delivery snapshot based only on route specification and itinerary.
      /// </summary>
      /// <param name="specification">Current route specification.</param>
      /// <param name="itinerary">Current itinerary.</param>
      /// <returns>Delivery status description.</returns>
      public static Delivery DerivedFrom(RouteSpecification specification, Itinerary itinerary)
      {
         return new Delivery(null, itinerary, specification);
      }

      /// <summary>
      /// Creates a new delivery snapshot based on the previous (current) one and (possibly changed)
      /// specification and itinerary.
      /// </summary>
      /// <param name="specification">Current route specification.</param>
      /// <param name="itinerary">Current itinerary.</param>
      /// <returns>Delivery status description.</returns>
      public Delivery Derive(RouteSpecification specification, Itinerary itinerary)
      {
         return new Delivery(LastEvent, itinerary, specification);
      }

      /// <summary>
      /// Creates a new delivery snapshot based on route specification, itinerary and last processed
      /// handling event.
      /// </summary>
      /// <param name="specification">Current route specification.</param>
      /// <param name="itinerary">Current itinerary.</param>
      /// <param name="lastEvent">Last processed handling event.</param>
      /// <returns>Delivery status description.</returns>
      public static Delivery DerivedFrom(RouteSpecification specification, Itinerary itinerary, HandlingEvent lastEvent)
      {
         return new Delivery(lastEvent, itinerary, specification);
      }

      private Delivery(HandlingEvent lastHandlingEvent, Itinerary itinerary, RouteSpecification specification)
      {
         CalculatedAt = DateTime.Now;
         LastEvent = lastHandlingEvent;

         IsMisdirected = CalculateMisdirectionStatus(itinerary, lastHandlingEvent);
         RoutingStatus = CalculateRoutingStatus(itinerary, specification);
         TransportStatus = CalculateTransportStatus(lastHandlingEvent);
         LastKnownLocation = CalculateLastKnownLocation(lastHandlingEvent);
         EstimatedTimeOfArrival = CalculateEta(itinerary);
         NextExpectedActivity = CalculateNextExpectedActivity(specification, itinerary, lastHandlingEvent);
         IsUnloadedAtDestination = CalculateUnloadedAtDestination(specification, lastHandlingEvent);
      }

      private static bool CalculateUnloadedAtDestination(RouteSpecification specification, HandlingEvent lastHandlingEvent)
      {
         return lastHandlingEvent != null &&
                  lastHandlingEvent.EventType == HandlingEventType.Unload &&
                  specification.Destination == lastHandlingEvent.Location;
      }

      private DateTime? CalculateEta(Itinerary itinerary)
      {
         return OnTrack ? itinerary.FinalArrivalDate : null;
      }

      private static UnLocode CalculateLastKnownLocation(HandlingEvent lastHandlingEvent)
      {
         return lastHandlingEvent != null ? lastHandlingEvent.Location : null;
      }

      private static TransportStatus CalculateTransportStatus(HandlingEvent lastHandlingEvent)
      {
         if (lastHandlingEvent == null)
         {
            return TransportStatus.NotReceived;
         }

         switch (lastHandlingEvent.EventType)
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

      private HandlingActivity CalculateNextExpectedActivity(RouteSpecification routeSpecification, Itinerary itinerary, HandlingEvent lastHandlingEvent)
      {
         if (!OnTrack)
         {
            return null;
         }

         if (lastHandlingEvent == null)
         {
            return new HandlingActivity(HandlingEventType.Receive, routeSpecification.Origin);
         }

         switch (lastHandlingEvent.EventType)
         {
            case HandlingEventType.Load:

               Leg lastLeg = itinerary.Legs.FirstOrDefault(x => x.LoadLocation == lastHandlingEvent.Location);
               return lastLeg != null ? new HandlingActivity(HandlingEventType.Unload, lastLeg.UnloadLocation) : null;

            case HandlingEventType.Unload:
               IEnumerator<Leg> enumerator = itinerary.Legs.GetEnumerator();
               while (enumerator.MoveNext())
               {
                  if (enumerator.Current.UnloadLocation == lastHandlingEvent.Location)
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

      private static bool CalculateMisdirectionStatus(Itinerary itinerary, HandlingEvent lastEvent)
      {
         if (lastEvent == null)
         {
            return false;
         }
         return !itinerary.IsExpected(lastEvent);
      }

      private bool OnTrack
      {
         get { return RoutingStatus == RoutingStatus.Routed && !IsMisdirected; }
      }

      #region Infrastructure
      protected Delivery()
      {
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return CalculatedAt;
         yield return EstimatedTimeOfArrival;
         yield return LastEvent;
         yield return IsUnloadedAtDestination;
         yield return IsUnloadedAtDestination;
         yield return LastKnownLocation;
         yield return IsMisdirected;
         yield return RoutingStatus;
         yield return TransportStatus;
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
