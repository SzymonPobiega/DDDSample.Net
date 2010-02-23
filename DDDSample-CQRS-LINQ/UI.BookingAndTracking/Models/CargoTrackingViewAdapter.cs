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
                  return Resources.Messages.cargo_status_IN_PORT.UIFormat(_cargo.CurrentInformation.LastKnownLocation);
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
            string location = _cargo.CurrentInformation.NextExpectedLocation;
            HandlingEventType? eventType = _cargo.CurrentInformation.NextExpectedEventType;
            if (eventType == null)
            {
               return "";
            }

            const string text = "Next expected activity is to ";
            if (eventType == HandlingEventType.Load)
            {
               return
                  text + eventType.ToString().ToLower() + " cargo onto voyage XXX" +
                  " in " + location;
            }
            if (eventType == HandlingEventType.Unload)
            { 
               return
                  text + eventType.ToString().ToLower() + " cargo off of XXX" +
                  " in " + eventType;
            }
            return text + eventType.ToString().ToLower() + " cargo in " + location;
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
            DateTime? eta = _cargo.CurrentInformation.Eta;
            return eta.HasValue ? eta.ToString() : "?";
         }
      }      
   }
}