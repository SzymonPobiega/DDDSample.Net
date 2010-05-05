using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Single cargo handling event.
   /// </summary>   
   [Serializable]
   public class HandlingEvent
   {      
      private readonly HandlingEventType _eventType;
      private readonly UnLocode _location;      
      private readonly DateTime _registrationDate;
      private readonly DateTime _completionDate;

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      public HandlingEvent(HandlingEventType eventType, UnLocode location, DateTime registrationDate, DateTime completionDate)
      {
         _eventType = eventType;
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
      public virtual UnLocode Location
      {
         get { return _location; }
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