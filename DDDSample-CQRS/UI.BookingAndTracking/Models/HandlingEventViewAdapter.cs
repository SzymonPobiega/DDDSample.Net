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
         get { return _handlingEvent.LastKnownActivity.Location; }
      }

      public string Time
      {
         get { return _handlingEvent.CalculatedAt.ToString(); }
      }

      public string Type
      {
         get { return _handlingEvent.LastKnownActivity.EventType.ToString(); }
      }

      public bool IsExpected
      {
         get { return !_handlingEvent.IsMisdirected; }
      }

      public string Description
      {
         get
         {            
            if (_handlingEvent.LastKnownActivity == null)
            {
               return "Registered";
            }
            switch (_handlingEvent.LastKnownActivity.EventType)
            {
               case HandlingEventType.Load:
                  return Resources.Messages.eventDescription_LOAD.UIFormat("XXX", _handlingEvent.LastKnownActivity.Location,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Unload:
                  return Resources.Messages.eventDescription_UNLOAD.UIFormat("XXX", _handlingEvent.LastKnownActivity.Location,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Receive:
                  return Resources.Messages.eventDescription_RECEIVE.UIFormat(_handlingEvent.LastKnownActivity.Location,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Claim:
                  return Resources.Messages.eventDescription_RECEIVE.UIFormat(_handlingEvent.LastKnownActivity.Location,
                                                                           _handlingEvent.CalculatedAt);
               case HandlingEventType.Customs:
                  return Resources.Messages.eventDescription_CUSTOMS.UIFormat(_handlingEvent.LastKnownActivity.Location,
                                                                           _handlingEvent.CalculatedAt);
            }
            throw new NotSupportedException();
         }
      }
   }
}