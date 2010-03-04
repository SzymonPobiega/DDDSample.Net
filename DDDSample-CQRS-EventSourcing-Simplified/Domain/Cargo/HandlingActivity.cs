using System;
using System.Collections.Generic;
using DDDSample.Domain.Location;

namespace DDDSample.Domain.Cargo
{
   /// <summary>
   /// A handling activity represents how and where a cargo can be handled,
   /// and can be used to express predictions about what is expected to
   /// happen to a cargo in the future.
   /// </summary>
#pragma warning disable 661,660 //Equals and GetHashCode are overridden in ValueObject class.
   [Serializable]
   public class HandlingActivity : ValueObject
#pragma warning restore 661,660
   {
      private readonly HandlingEventType _eventType;
      private readonly UnLocode _location;

      public HandlingActivity(HandlingEventType eventType, UnLocode location)
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

      public UnLocode Location
      {
         get { return _location; }
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return EventType;
         yield return Location;
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