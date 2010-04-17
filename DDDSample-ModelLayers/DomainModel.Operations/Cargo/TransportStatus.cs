using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.DomainModel.Operations.Cargo
{
   /// <summary>
   /// Describes status of cargo transportation.
   /// </summary>
   public enum TransportStatus
   {
      /// <summary>
      /// Cargo hasn't been received yet.
      /// </summary>
      NotReceived,
      /// <summary>
      /// Cargo is onboard carrier.
      /// </summary>
      OnboardCarrier,
      /// <summary>
      /// Cargo is in port.
      /// </summary>
      InPort,
      /// <summary>
      /// Cargo has been claimed.
      /// </summary>
      Claimed,
      /// <summary>
      /// Cargo transport state is unknown.
      /// </summary>
      Unknown
   }
}