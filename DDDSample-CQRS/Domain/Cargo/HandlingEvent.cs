using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Single cargo handling event.
   /// </summary>   
   public class HandlingEvent
   {      
      private readonly HandlingEventType _eventType;
      private readonly Location.Location _location;      
      private readonly DateTime _registrationDate;
      private readonly DateTime _completionDate;
      private readonly Cargo _parent;

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      public HandlingEvent(HandlingEventType eventType, Location.Location location, DateTime registrationDate, DateTime completionDate, Cargo parent)
      {
         _eventType = eventType;
         _parent = parent;
         _completionDate = completionDate;
         _registrationDate = registrationDate;         
         _location = location;         
      }

      /// <summary>
      /// Date when action represented by the event was completed.
      /// </summary>
      public virtual DateTime CompletionDate
      {
         get { return _completionDate; }
      }

      /// <summary>
      /// Date when event was registered.
      /// </summary>
      public virtual DateTime RegistrationDate
      {
         get { return _registrationDate; }
      }
      
      /// <summary>
      /// Location where event occured.
      /// </summary>
      public virtual Location.Location Location
      {
         get { return _location; }
      }

      public virtual TrackingId TrackingId
      {
         get { return _parent.TrackingId; }
      }

      /// <summary>
      /// Type of the event.
      /// </summary>
      public virtual HandlingEventType EventType
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