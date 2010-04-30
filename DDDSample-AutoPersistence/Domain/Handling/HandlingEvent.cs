using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace DDDSample.Domain.Handling
{
   /// <summary>
   /// Single cargo handling event.
   /// </summary>   
   public class HandlingEvent : IEntity<HandlingHistory>
   {
      public virtual Guid Id { get; protected set; }

      protected virtual HandlingHistory HandlingHistory { get; set;}

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      public HandlingEvent(HandlingEventType eventType, Location.Location location, DateTime registrationDate, DateTime completionDate, HandlingHistory parent)
      {
         EventType = eventType;
         HandlingHistory = parent;
         CompletionDate = completionDate;
         RegistrationDate = registrationDate;         
         Location = location;         
      }

      /// <summary>
      /// Date when action represented by the event was completed.
      /// </summary>
      public virtual DateTime CompletionDate { get; protected set; }

      /// <summary>
      /// Date when event was registered.
      /// </summary>
      public virtual DateTime RegistrationDate { get; protected set; }

      /// <summary>
      /// Location where event occured.
      /// </summary>
      public virtual Location.Location Location { get; protected set; }

      /// <summary>
      /// Type of the event.
      /// </summary>
      public virtual HandlingEventType EventType { get; protected set; }

      public virtual TrackingId TrackingId
      {
         get { return HandlingHistory.TrackingId; }
      }


      /// <summary>
      /// Required by NHibernate.
      /// </summary>
      protected HandlingEvent()
      {         
      }
   }
}