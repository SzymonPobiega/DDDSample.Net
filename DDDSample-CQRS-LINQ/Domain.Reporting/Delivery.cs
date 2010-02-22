using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Reporting
{
   /// <summary>
   /// Description of delivery status.
   /// </summary>
   public class Delivery
   {
      public Delivery(Cargo parent, HandlingActivity nextExpectedActivity, HandlingActivity lastKnownActivity, RoutingStatus routingStatus, TransportStatus transportStatus, DateTime? estimatedTimeOfArrival, bool isUnloadedAtDestination, bool isMisdirected, DateTime calculatedAt)
      {
         Parent = parent;
         NextExpectedActivity = nextExpectedActivity;
         LastKnownActivity = lastKnownActivity;
         RoutingStatus = routingStatus;
         TransportStatus = transportStatus;
         EstimatedTimeOfArrival = estimatedTimeOfArrival;
         IsUnloadedAtDestination = isUnloadedAtDestination;
         IsMisdirected = isMisdirected;
         CalculatedAt = calculatedAt;
      }

      public virtual Cargo Parent { get; protected set; }

      /// <summary>
      /// Gets next expected activity.
      /// </summary>
      public virtual HandlingActivity NextExpectedActivity { get; protected set; }

      /// <summary>
      /// Gets last known activity of this cargo.
      /// </summary>
      public virtual HandlingActivity LastKnownActivity { get; protected set; }

      /// <summary>
      /// Gets status of cargo routing.
      /// </summary>
      public virtual RoutingStatus RoutingStatus { get; protected set; }

      /// <summary>
      /// Gets status of cargo transport.
      /// </summary>
      public virtual TransportStatus TransportStatus { get; protected set; }

      /// <summary>
      /// Gets estimated time of arrival. Returns null if information cannot be obtained (cargo is misrouted).
      /// </summary>
      public virtual DateTime? EstimatedTimeOfArrival { get; protected set; }            

      /// <summary>
      /// Gets if this cargo has been unloaded at its destination.
      /// </summary>
      public virtual bool IsUnloadedAtDestination { get; protected set; }

      /// <summary>
      /// Gets if this cargo was misdirected.
      /// </summary>
      public virtual bool IsMisdirected { get; protected set; }

      /// <summary>
      /// Gets time when this delivery status was calculated.
      /// </summary>
      public virtual DateTime CalculatedAt { get; protected set; }      

      protected Delivery()
      {
      }      
   }
}