using System;
using System.Collections.Generic;

namespace DDDSample.DomainModel.Potential.Voyage
{
#pragma warning disable 660,661
   public class CarrierMovement : ValueObject
#pragma warning restore 660,661
   {
      private readonly TransportLeg _transportLeg;
      private readonly DateTime _departureTime;
      private readonly DateTime _arrivalTime;

      public CarrierMovement(TransportLeg transportLeg, DateTime departureTime, DateTime arrivalTime)
      {
         _transportLeg = transportLeg;
         _arrivalTime = arrivalTime;
         _departureTime = departureTime;
      }

      public DateTime DepartureTime
      {
         get { return _departureTime; }
      }

      public DateTime ArrivalTime
      {
         get { return _arrivalTime; }
      }

      public TransportLeg TransportLeg
      {
         get { return _transportLeg; }
      }

      public static bool operator ==(CarrierMovement left, CarrierMovement right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(CarrierMovement left, CarrierMovement right)
      {
         return NotEqualOperator(left, right);
      }      

      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return DepartureTime;
         yield return ArrivalTime;
         yield return TransportLeg;
      }

      protected CarrierMovement()
      {
      }
   }
}