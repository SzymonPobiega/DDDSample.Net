using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Cargo;

namespace DDDSample.DomainModel.Operations.Handling
{
   /// <summary>
   /// Single cargo handling event.
   /// </summary>   
   public class HandlingEvent
   {
      private readonly HandlingEventType _eventType;
      private readonly DomainModel.Potential.Location.Location _location;      
      private readonly DateTime _registrationDate;
      private readonly DateTime _completionDate;
      protected virtual HandlingHistory _parent { get; set;}

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      public HandlingEvent(HandlingEventType eventType, DomainModel.Potential.Location.Location location, DateTime registrationDate, DateTime completionDate, HandlingHistory parent)
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
      
      /// <summary>
      /// Location where event occured.
      /// </summary>
      public DomainModel.Potential.Location.Location Location
      {
         get { return _location; }
      }

      public TrackingId TrackingId
      {
         get { return _parent.TrackingId; }
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