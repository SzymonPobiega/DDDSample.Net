using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using Newtonsoft.Json;

namespace DDDSample.Reporting
{
   public class Cargo
   {
      public string Id { get; set; }
      public string TrackingId { get; protected set; }
      public string AggregateId { get; protected set; }

      public string Origin { get; protected set; }
      public string Destination { get; protected set; }
      public DateTime ArrivalDeadline { get; protected set; }
      public Delivery CurrentInformation { get; protected set; }

      public List<Leg> RouteSpecification { get; set; }

      private IList<Delivery> History { get; set; }

      public Cargo(string aggregateId, string trackingId, string origin, string destination, DateTime arrivalDeadline)
      {
         AggregateId = aggregateId;
         TrackingId = trackingId;
         Origin = origin;
         Destination = destination;
         ArrivalDeadline = arrivalDeadline;

         History = new List<Delivery>();
         UpdateHistory(null, null, RoutingStatus.NotRouted, TransportStatus.NotReceived, null, false, false,
                       DateTime.Now);
      }

      public void UpdateRouteSpecification(string origin, string destination, DateTime arrivalDeadline)
      {
         Origin = origin;
         Destination = destination;
         ArrivalDeadline = arrivalDeadline;
         RouteSpecification = null;
      }

      public void UpdateHistory(HandlingActivity nextExpectedActivity, HandlingActivity lastKnownActivity, RoutingStatus routingStatus, TransportStatus transportStatus, DateTime? estimatedTimeOfArrival, bool isUnloadedAtDestination, bool isMisdirected, DateTime calculatedAt)
      {
         var delivery = new Delivery(nextExpectedActivity, lastKnownActivity, routingStatus, transportStatus, estimatedTimeOfArrival, isUnloadedAtDestination, isMisdirected, calculatedAt);
         History.Add(delivery);
         CurrentInformation = delivery;
      }

      [JsonIgnore]
      public IEnumerable<Delivery> DeliveryHistory
      {
         get { return History; }
      }

      protected Cargo()
      {
      }
   }
}