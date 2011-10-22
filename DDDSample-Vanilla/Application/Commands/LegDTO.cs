using System;

namespace DDDSample.Application.Commands
{
   public class LegDTO
   {
      public DateTime UnloadTime { get; set; }
      public DateTime LoadTime { get; set; }
      public string To { get; set; }
      public string From { get; set; }
      public string VoyageNumber { get; set; }
      public Guid VoyageId { get; set; }
   }
}