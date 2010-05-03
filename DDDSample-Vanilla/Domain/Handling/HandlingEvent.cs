using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace DDDSample.Domain.Handling
{
   /// <summary>
   /// Single cargo handling event.
   /// </summary>   
   public class HandlingEvent
   {
      /// <summary>
      /// Cargo which this handling event is concerned.
      /// </summary>
      public virtual Cargo.Cargo Cargo { get; set;}
      /// <summary>
      /// Type of the event.
      /// </summary>
      public virtual HandlingEventType EventType { get; set; }
      /// <summary>
      /// Location where event occured.
      /// </summary>
      public virtual Location.Location Location { get; set; }
      /// <summary>
      /// Date when event was registered.
      /// </summary>
      public virtual DateTime RegistrationDate { get; set; }
      /// <summary>
      /// Date when action represented by the event was completed.
      /// </summary>
      public virtual DateTime CompletionDate { get; set; }
      /// <summary>
      /// Unique id of this event.
      /// </summary>
      public virtual Guid Id { get; protected set; }

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      /// <param name="cargo"></param>
      public HandlingEvent(HandlingEventType eventType, Location.Location location, 
         DateTime registrationDate, DateTime completionDate, Cargo.Cargo cargo)
      {
         EventType = eventType;
         Location = location;
         RegistrationDate = registrationDate;
         CompletionDate = completionDate;
         Cargo = cargo;

         DomainEvents.Raise(new CargoWasHandledEvent(this));
      }

      /// <summary>
      /// Required by NHibernate.
      /// </summary>
      protected HandlingEvent()
      {         
      }
   }
}