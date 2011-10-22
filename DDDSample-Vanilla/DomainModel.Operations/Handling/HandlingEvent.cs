using System;
using DDDSample.DomainModel.Potential.Location;

namespace DDDSample.DomainModel.Operations.Handling
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
      public virtual Location Location { get; set; }
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
       /// <param name="eventPublisher"></param>
       public HandlingEvent(HandlingEventType eventType, Location location, 
         DateTime registrationDate, DateTime completionDate, Cargo.Cargo cargo, IEventPublisher eventPublisher)
      {
         EventType = eventType;
         Location = location;
         RegistrationDate = registrationDate;
         CompletionDate = completionDate;
         Cargo = cargo;

         eventPublisher.Raise(new CargoWasHandledEvent(this));
      }

      /// <summary>
      /// Required by NHibernate.
      /// </summary>
      protected HandlingEvent()
      {         
      }
   }
}