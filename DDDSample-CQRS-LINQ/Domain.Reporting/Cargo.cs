using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using DDDSample.Messages;

namespace DDDSample.Reporting
{
   public partial class Cargo
   {      
      public Cargo(string trackingId, string origin, string destination, DateTime arrivalDeadline)
         : this()
      {
         Id = Guid.NewGuid();
         TrackingId = trackingId;
         Origin = origin;
         Destination = destination;
         ArrivalDeadline = arrivalDeadline;
         UpdateHistory(null, null, null, null, RoutingStatus.NotRouted, TransportStatus.NotReceived, null, false, false,
                       DateTime.Now);                  
      }

      public virtual void UpdateRouteSpecification(string origin, string destination, DateTime arrivalDeadline)
      {
         Origin = origin;
         Destination = destination;
         ArrivalDeadline = arrivalDeadline;
         RouteSpecification = null;
      }

      public virtual void UpdateHistory(HandlingEventType? nextExpectedEvent, string nextExpectedLocation, HandlingEventType? lastKnownEvent, string lastKnownLocation, RoutingStatus routingStatus, TransportStatus transportStatus, DateTime? estimatedTimeOfArrival, bool isUnloadedAtDestination, bool isMisdirected, DateTime calculatedAt)
      {
         Delivery delivery = new Delivery(this, nextExpectedEvent, nextExpectedLocation, lastKnownEvent, lastKnownLocation, routingStatus, transportStatus, estimatedTimeOfArrival, isUnloadedAtDestination, isMisdirected, calculatedAt);
         DeliveryHistory.Add(delivery);
         CurrentInformation = delivery;
      }

      public Delivery CurrentInformation
      {
         get { return _Deliveries.Single(x => x.Id == CurrentInformationId); }
         set { CurrentInformationId = value.Id; }
      }
   }
}