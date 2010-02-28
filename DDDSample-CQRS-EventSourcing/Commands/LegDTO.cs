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
   }
}