using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Commands
{
   [Serializable]
   public class LegDTO
   {
      public string LoadLocation { get; set; }
      public string UnloadLocation { get; set; }
      public DateTime LoadDate { get; set; }
      public DateTime UnloadDate { get; set; }

      public LegDTO()
      {         
      }

      public LegDTO(string loadLocation, DateTime loadDate, string unloadLocation, DateTime unloadDate)
      {
         LoadLocation = loadLocation;
         LoadDate = loadDate;
         UnloadLocation = unloadLocation;
         UnloadDate = unloadDate;
      }
   }
}