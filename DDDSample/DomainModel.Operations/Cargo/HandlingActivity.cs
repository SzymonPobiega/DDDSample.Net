using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Operations.Handling;

namespace DDDSample.DomainModel.Operations.Cargo
{
   public class HandlingActivity : ValueObject
#pragma warning restore 661,660
   {
      private readonly HandlingEventType _eventType;
      private readonly DomainModel.Potential.Location.Location _location;

      public HandlingActivity(HandlingEventType eventType, DomainModel.Potential.Location.Location location)
      {
         if (location == null)
         {
            throw new ArgumentNullException("location");
         }
         _eventType = eventType;
         _location = location;
      }

      public HandlingEventType EventType
      {
         get { return _eventType; }
      }

      public DomainModel.Potential.Location.Location Location
      {
         get { return _location; }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return EventType;
         yield return Location.UnLocode;
      }

      public static bool operator ==(HandlingActivity left, HandlingActivity right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(HandlingActivity left, HandlingActivity right)
      {
         return NotEqualOperator(left, right);
      }   
   
      protected HandlingActivity()
      {         
      }
   }
}