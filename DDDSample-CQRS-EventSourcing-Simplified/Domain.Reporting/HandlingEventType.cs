using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Reporting
{
   /// <summary>
   /// Type of handling event.
   /// </summary>
   public enum HandlingEventType
   {
      Load,
      Unload,
      Receive,
      Claim,
      Customs
   }
}