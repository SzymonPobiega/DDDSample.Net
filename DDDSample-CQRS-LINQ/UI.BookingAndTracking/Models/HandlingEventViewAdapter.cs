using System;
using System.Linq;
using DDDSample.Reporting;

namespace DDDSample.UI.BookingAndTracking.Models
{
   public class HandlingEventViewAdapter
   {
      private readonly Delivery _handlingEvent;

      public HandlingEventViewAdapter(Delivery handlingEvent)
      {
         _handlingEvent = handlingEvent;
      }

      public string Location
      {
         get { return _handlingEvent.LastKnownLocation; }
      }

      public string Time
      {
         get { return _handlingEvent.CalculatedAt.ToString(); }
      }

      public string Type
      {
         get { return _handlingEvent.LastKnownEventType.ToString(); }
      }

      public bool IsExpected
      {
         get { return !_handlingEvent.IsMisdirected; }
      }

      public string Description
      {
         get
         {            
            if (_handlingEvent.LastKnownEventType == null)
            {
               return "Registered";
            }
            switch (_handlingEvent.LastKnownEventType)
            {
               case HandlingEventType.Load:
                  return Resources.Messages.eventDescription_LOAD.UIFormat("XXX", _handlingEvent.LastKnownLocation,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Unload:
                  return Resources.Messages.eventDescription_UNLOAD.UIFormat("XXX", _handlingEvent.LastKnownLocation,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Receive:
                  return Resources.Messages.eventDescription_RECEIVE.UIFormat(_handlingEvent.LastKnownLocation,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Claim:
                  return Resources.Messages.eventDescription_RECEIVE.UIFormat(_handlingEvent.LastKnownLocation,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Customs:
                  return Resources.Messages.eventDescription_CUSTOMS.UIFormat(_handlingEvent.LastKnownLocation,
                                                                           _handlingEvent.CalculatedAt);
            }
            throw new NotSupportedException();
         }
      }
   }
}