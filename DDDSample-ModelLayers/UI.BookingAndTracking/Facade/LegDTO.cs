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

      public LegDTO(string voyageNumber, Guid voyageId, string from, string to, DateTime loadTime, DateTime unloadTime)
      {
         VoyageNumber = voyageNumber;
         VoyageId = voyageId;
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
      public Guid VoyageId { get; set; }
   }
}