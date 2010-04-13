using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using HandlingEvent=DDDSample.DomainModel.Operations.Handling.HandlingEvent;

namespace DDDSample.UI.BookingAndTracking.Models
{   
   public class CargoTrackingViewAdapter
   {
      private readonly Cargo _cargo;
      private readonly IList<HandlingEvent> _handlingEvents;      

      public CargoTrackingViewAdapter(Cargo cargo, HandlingHistory handlingHistory)
      {
         _cargo = cargo;
         if (handlingHistory != null)
         {
            _handlingEvents = handlingHistory.EventsByCompletionTime.ToList();  
         }
         else
         {
            _handlingEvents = new List<HandlingEvent>();
         }
      }

      public string TrackingId
      {
         get { return _cargo.TrackingId.IdString; }
      }

      public IEnumerable<HandlingEventViewAdapter> HandlingEvents
      {
         get { return _handlingEvents.Select(x => new HandlingEventViewAdapter(x, _cargo)); }
      }

      public string StatusText
      {
         get
         {                       
            switch (_cargo.Delivery.TransportStatus)
            {
               case TransportStatus.InPort:
                  return Resources.Messages.cargo_status_IN_PORT.UIFormat(_cargo.Delivery.LastKnownLocation.Name);
               case TransportStatus.OnboardCarrier:
                  return Resources.Messages.cargo_status_ONBOARD_CARRIER.UIFormat("XXX");
               case TransportStatus.Claimed:
                  return Resources.Messages.cargo_status_CLAIMED.UIFormat();
               case TransportStatus.NotReceived:
                  return Resources.Messages.cargo_status_NOT_RECEIVED.UIFormat();
               case TransportStatus.Unknown:
                  return Resources.Messages.cargo_status_UNKNOWN.UIFormat();
            }
            throw new NotSupportedException();
         }
      }


      public String NextExpectedActivity
      {
         get
         {
            HandlingActivity activity = _cargo.Delivery.NextExpectedActivity;
            if (activity == null)
            {
               return "";
            }

            const string text = "Next expected activity is to ";
            HandlingEventType type = activity.EventType;
            if (type == HandlingEventType.Load)
            {
               return
                  text + type.ToString().ToLower() + " cargo onto voyage XXX" +
                  " in " + activity.Location.Name;
            }
            if (type == HandlingEventType.Unload)
            { 
               return
                  text + type.ToString().ToLower() + " cargo off of XXX" +
                  " in " + activity.Location.Name;
            }
            return text + type.ToString().ToLower() + " cargo in " + activity.Location.Name;
         }
      }

      public string Destination
      {
         get
         {
            return _cargo.RouteSpecification.Destination.Name;
         }
      }

      public string Eta
      {
         get
         {
            DateTime? eta = _cargo.Delivery.EstimatedTimeOfArrival;
            return eta.HasValue ? eta.ToString() : "?";
         }
      }      
   }
}