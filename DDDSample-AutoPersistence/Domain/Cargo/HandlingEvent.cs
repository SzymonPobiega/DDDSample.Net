using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// Represents handling event from viewpoint of cargo aggregate
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   public class HandlingEvent : ValueObject, IAggregateMember<Cargo>
#pragma warning restore 661,660
   {
      private readonly HandlingEventType _eventType;
      private readonly Location.Location _location;
      private readonly DateTime _registrationDate;
      private readonly DateTime _completionDate;

      /// <summary>
      /// Creates new event.
      /// </summary>
      /// <param name="eventType"></param>
      /// <param name="location"></param>
      /// <param name="registrationDate"></param>
      /// <param name="completionDate"></param>
      public HandlingEvent(HandlingEventType eventType, Location.Location location, DateTime registrationDate, DateTime completionDate)
      {
         _eventType = eventType;         
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
      /// Location UnLocode where event occured.
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

      #region Infrastructure
      protected HandlingEvent()
      {         
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _eventType;
         yield return _location;
         yield return _registrationDate;
         yield return _completionDate;
      }

      public static bool operator ==(HandlingEvent left, HandlingEvent right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(HandlingEvent left, HandlingEvent right)
      {
         return NotEqualOperator(left, right);
      }
      #endregion
   }
}