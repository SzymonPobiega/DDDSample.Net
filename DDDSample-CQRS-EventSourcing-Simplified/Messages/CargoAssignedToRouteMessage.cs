using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace DDDSample.Messages
{
   /// <summary>
   /// Message representing an event informing that cargo has been assigned to a new route.
   /// </summary>
   [Serializable]
   public class CargoAssignedToRouteMessage : IMessage
   {
      public Guid CargoId { get; set; }
      public List<LegDTO> Legs { get; set; }
   }
}