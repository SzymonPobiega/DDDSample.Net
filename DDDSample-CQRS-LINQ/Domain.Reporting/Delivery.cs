using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Reporting
{
   /// <summary>
   /// Description of delivery status.
   /// </summary>
   public partial class Delivery
   {
      public Delivery(Cargo parent, HandlingEventType? nextExpectedEvent, string nextExpectedLocation, HandlingEventType? lastKnownEvent, string lastKnownLocation, RoutingStatus routingStatus, TransportStatus transportStatus, DateTime? estimatedTimeOfArrival, bool isUnloadedAtDestination, bool isMisdirected, DateTime calculatedAt)
         : this()
      {
         Id = Guid.NewGuid();
         Cargo = parent;
         NextExpectedEventType = nextExpectedEvent;
         NextExpectedLocation = nextExpectedLocation;
         LastKnownEventType = lastKnownEvent;
         LastKnownLocation = lastKnownLocation;
         RoutingStatus = routingStatus;
         TransportStatus = transportStatus;
         Eta = estimatedTimeOfArrival;
         UnloadedAtDest = isUnloadedAtDestination;
         IsMisdirected = isMisdirected;
         CalculatedAt = calculatedAt;
      }      
   }
}