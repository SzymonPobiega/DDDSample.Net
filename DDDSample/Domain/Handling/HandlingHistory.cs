using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;

namespace DDDSample.Domain.Handling
{
   /// <summary>
   /// Contains information about cargo handling history. Enables registration of cargo
   /// handling events.
   /// </summary>
   public class HandlingHistory
   {
      private readonly IList<HandlingEvent> _events;

      public HandlingHistory(TrackingId cargoTrackingId)
      {
         TrackingId = cargoTrackingId;
         _events = new List<HandlingEvent>();
      }      

      /// <summary>
      /// Registers new handling event into the history.
      /// </summary>
      /// <param name="eventType">Type of the event.</param>
      /// <param name="location">Location where event occured.</param>
      /// <param name="registrationDate">Date when event was registered.</param>
      /// <param name="completionDate">Date when action represented by the event was completed.</param>
      public virtual void RegisterHandlingEvent(HandlingEventType eventType, Location.Location location, DateTime registrationDate, DateTime completionDate)
      {         
         HandlingEvent @event = new HandlingEvent(eventType, location, registrationDate, completionDate,this);
         _events.Add(@event);
         DomainEvents.Raise(new CargoWasHandledEvent(@event));
      }

      /// <summary>
      /// Gets a collection of events ordered by their completion time.
      /// </summary>
      public virtual IEnumerable<HandlingEvent> EventsByCompletionTime
      {
         get { return _events.OrderBy(x => x.CompletionDate);}
      }

      /// <summary>
      /// Gets tracking id of cargo which this history object belongs to.
      /// </summary>
      public virtual TrackingId TrackingId { get; protected set;}      

      /// <summary>
      /// Required by NHibernate.
      /// </summary>
      protected HandlingHistory()
      {
      }      
   }
}
