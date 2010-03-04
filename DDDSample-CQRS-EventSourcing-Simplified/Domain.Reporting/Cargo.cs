using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Messages;

namespace DDDSample.Reporting
{
   public class Cargo
   {
      public virtual Guid Id { get; protected set; }
      public virtual string TrackingId { get; protected set; }

      public virtual string Origin { get; protected set; }
      public virtual string Destination { get; protected set; }
      public virtual DateTime ArrivalDeadline { get; protected set; }
      public virtual Delivery CurrentInformation { get; protected set; }

      public virtual List<LegDTO> RouteSpecification { get; set; }

      private readonly IList<Delivery> _history;            

      public Cargo(Guid id, string trackingId, string origin, string destination, DateTime arrivalDeadline)
      {
         Id = id;
         TrackingId = trackingId;
         Origin = origin;
         Destination = destination;
         ArrivalDeadline = arrivalDeadline;

         _history = new List<Delivery>();
         UpdateHistory(null, null, RoutingStatus.NotRouted, TransportStatus.NotReceived, null, false, false,
                       DateTime.Now);                  
      }

      public virtual void UpdateRouteSpecification(string origin, string destination, DateTime arrivalDeadline)
      {
         Origin = origin;
         Destination = destination;
         ArrivalDeadline = arrivalDeadline;
         RouteSpecification = null;
      }

      public virtual void UpdateHistory(HandlingActivity nextExpectedActivity, HandlingActivity lastKnownActivity, RoutingStatus routingStatus, TransportStatus transportStatus, DateTime? estimatedTimeOfArrival, bool isUnloadedAtDestination, bool isMisdirected, DateTime calculatedAt)
      {
         Delivery delivery = new Delivery(this, nextExpectedActivity, lastKnownActivity, routingStatus, transportStatus, estimatedTimeOfArrival, isUnloadedAtDestination, isMisdirected, calculatedAt);
         _history.Add(delivery);
         CurrentInformation = delivery;
      }

      public virtual IEnumerable<Delivery> DeliveryHistory
      {
         get { return _history; }
      }

      protected Cargo()
      {         
      }
   }
}