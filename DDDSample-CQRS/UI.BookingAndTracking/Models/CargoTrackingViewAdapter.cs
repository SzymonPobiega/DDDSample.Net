using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Reporting;

namespace DDDSample.UI.BookingAndTracking.Models
{   
   public class CargoTrackingViewAdapter
   {
      private readonly Cargo _cargo;
      
      public CargoTrackingViewAdapter(Cargo cargo)
      {
         _cargo = cargo;         
      }

      public string TrackingId
      {
         get { return _cargo.TrackingId; }
      }

      public IEnumerable<HandlingEventViewAdapter> HandlingEvents
      {
         get { return _cargo.DeliveryHistory.Select(x => new HandlingEventViewAdapter(x)); }
      }

      public string StatusText
      {
         get
         {                       
            switch (_cargo.CurrentInformation.TransportStatus)
            {
               case TransportStatus.InPort:
                  return Resources.Messages.cargo_status_IN_PORT.UIFormat(_cargo.CurrentInformation.LastKnownActivity.Location);
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
            HandlingActivity activity = _cargo.CurrentInformation.NextExpectedActivity;
            if (activity == null)
            {
               return "";
            }

            const string text = "Next expected activity is to ";
            HandlingEventType? type = activity.EventType;
            if (type == HandlingEventType.Load)
            {
               return
                  text + type.ToString().ToLower() + " cargo onto voyage XXX" +
                  " in " + activity.Location;
            }
            if (type == HandlingEventType.Unload)
            { 
               return
                  text + type.ToString().ToLower() + " cargo off of XXX" +
                  " in " + activity.Location;
            }
            return text + type.ToString().ToLower() + " cargo in " + activity.Location;
         }
      }

      public string Destination
      {
         get
         {
            return _cargo.Destination;
         }
      }

      public string Eta
      {
         get
         {
            DateTime? eta = _cargo.CurrentInformation.EstimatedTimeOfArrival;
            return eta.HasValue ? eta.ToString() : "?";
         }
      }      
   }
}