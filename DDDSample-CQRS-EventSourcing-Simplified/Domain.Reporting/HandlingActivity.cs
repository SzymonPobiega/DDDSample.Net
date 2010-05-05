using System;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;

namespace DDDSample.Reporting
{
   /// <summary>
   /// A handling activity represents how and where a cargo can be handled,
   /// and can be used to express predictions about what is expected to
   /// happen to a cargo in the future.
   /// </summary>
   public class HandlingActivity
   {
      public HandlingEventType EventType { get; protected set; }
      public string Location { get; protected set;}

      public HandlingActivity(HandlingEventType eventType, string location)
      {
         EventType = eventType;
         Location = location;
      }
      
      protected HandlingActivity()
      {         
      }
   }
}