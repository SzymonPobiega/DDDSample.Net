using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace DDDSample.Commands
{
   [Serializable]
   public class BookNewCargoCommand
   {         
      public string Origin { get; set; }
      public string Destination { get; set; }
      public DateTime ArrivalDeadline { get; set; }
   }
}