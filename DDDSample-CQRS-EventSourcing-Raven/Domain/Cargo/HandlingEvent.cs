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
      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      public HandlingEvent(HandlingEventType eventType, UnLocode location, DateTime registrationDate, DateTime completionDate)
      {
         EventType = eventType;
         CompletionDate = completionDate;
         RegistrationDate = registrationDate;         
         Location = location;         
      }

      /// <summary>
      /// Date when action represented by the event was completed.
      /// </summary>
      public virtual DateTime CompletionDate { get; private set; }

      /// <summary>
      /// Date when event was registered.
      /// </summary>
      public virtual DateTime RegistrationDate { get; private set; }

      /// <summary>
      /// Location where event occured.
      /// </summary>
      public virtual UnLocode Location { get; private set; }

      /// <summary>
      /// Type of the event.
      /// </summary>
      public virtual HandlingEventType EventType { get; private set; }

      /// <summary>
      /// Required by NHibernate.
      /// </summary>
      protected HandlingEvent()
      {         
      }
   }
}