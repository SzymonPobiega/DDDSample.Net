using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain.Handling
{
   /// <summary>
   /// Single cargo handling event.
   /// </summary>
   public class HandlingEvent
   {
      private HandlingEventType _eventType;
      private Location.Location _location;      
      private DateTime _registrationDate;
      private DateTime _completionDate;
      protected virtual HandlingHistory _parent { get; set;}

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      internal HandlingEvent(HandlingEventType eventType, Location.Location location, DateTime registrationDate, DateTime completionDate, HandlingHistory parent)
      {
         _eventType = eventType;
         _parent = parent;
         _completionDate = completionDate;
         _registrationDate = registrationDate;         
         _location = location;
         //if (_eventType.RequiresVoyage())
         //{
         //   throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Event type {0} requires providing a voyage object.", eventType));
         //}
      }

      /// <summary>
      /// Date when action represented by the event was completed.
      /// </summary>
      public DateTime CompletionDate
      {
         get { return _completionDate; }
      }

      /// <summary>
      /// Date when event was registered.
      /// </summary>
      public DateTime RegistrationDate
      {
         get { return _registrationDate; }
      }

      public Cargo.Cargo Cargo
      {
         get { return _parent.Cargo; }
      }

      /// <summary>
      /// Location where event occured.
      /// </summary>
      public Location.Location Location
      {
         get { return _location; }
      }

      /// <summary>
      /// Type of the event.
      /// </summary>
      public HandlingEventType EventType
      {
         get { return _eventType; }
      }
      
      /// <summary>
      /// Required by NHibernate.
      /// </summary>
      protected HandlingEvent()
      {         
      }
   }
}