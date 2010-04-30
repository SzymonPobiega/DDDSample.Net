using System;
using System.Linq;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using HandlingEvent=DDDSample.Domain.Cargo.HandlingEvent;

namespace DDDSample.UI.BookingAndTracking.Models
{
   public class HandlingEventViewAdapter
   {
      private readonly Domain.Handling.HandlingEvent _handlingEvent;
      private readonly Cargo _cargo;

      public HandlingEventViewAdapter(Domain.Handling.HandlingEvent handlingEvent, Cargo cargo)
      {
         _handlingEvent = handlingEvent;
         _cargo = cargo;
      }

      public string Location
      {
         get { return _handlingEvent.Location.Name; }
      }

      public string Time
      {
         get { return _handlingEvent.CompletionDate.ToString(); }
      }

      public string Type
      {
         get { return _handlingEvent.EventType.ToString(); }
      }

      public bool IsExpected
      {
         get { return _cargo.Itinerary.IsExpected(new HandlingEvent(
            _handlingEvent.EventType,
            _handlingEvent.Location,
            _handlingEvent.RegistrationDate,
            _handlingEvent.CompletionDate)); }
      }

      public string Description
      {
         get
         {            
            switch (_handlingEvent.EventType)
            {
               case HandlingEventType.Load:
                  return Resources.Messages.eventDescription_LOAD.UIFormat("XXX", _handlingEvent.Location.Name,
                                                                           _handlingEvent.CompletionDate);
               case HandlingEventType.Unload:
                  return Resources.Messages.eventDescription_UNLOAD.UIFormat("XXX", _handlingEvent.Location.Name,
                                                                           _handlingEvent.CompletionDate);
               case HandlingEventType.Receive:
                  return Resources.Messages.eventDescription_RECEIVE.UIFormat(_handlingEvent.Location.Name,
                                                                           _handlingEvent.CompletionDate);
               case HandlingEventType.Claim:
                  return Resources.Messages.eventDescription_RECEIVE.UIFormat(_handlingEvent.Location.Name,
                                                                           _handlingEvent.CompletionDate);
               case HandlingEventType.Customs:
                  return Resources.Messages.eventDescription_CUSTOMS.UIFormat(_handlingEvent.Location.Name,
                                                                           _handlingEvent.CompletionDate);
            }
            throw new NotSupportedException();
         }
      }
   }
}