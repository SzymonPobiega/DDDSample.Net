using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   public class LegDTO
   {
      public LegDTO()
      {         
      }

      public LegDTO(string voyageNumber, string from, string to, DateTime loadTime, DateTime unloadTime)
      {
         VoyageNumber = voyageNumber;
         UnloadTime = unloadTime;
         LoadTime = loadTime;
         To = to;
         From = from;
      }

      public DateTime UnloadTime { get; set; }

      public DateTime LoadTime { get; set; }

      public string To { get; set; }

      public string From { get; set; }

      public string VoyageNumber { get; set; }
   }
}