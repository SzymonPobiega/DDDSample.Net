using System;
using System.Collections.Generic;
using System.Linq;

namespace DDDSample.DomainModel.Potential.Voyage
{
#pragma warning disable 660,661
   public class Schedule : ValueObject
#pragma warning restore 660,661
   {
      private readonly IList<CarrierMovement> _carrierMovements;
      private readonly DateTime _departureTime;
      private readonly DateTime _arrivalTime;

      public Schedule(IList<CarrierMovement> carrierMovements)
      {
         _carrierMovements = carrierMovements;
         _departureTime = _carrierMovements.First().DepartureTime;
         _arrivalTime = _carrierMovements.Last().ArrivalTime;
      }

      public IList<CarrierMovement> CarrierMovements
      {
         get { return _carrierMovements; }
      }

      public DateTime DepartureTime
      {
         get { return _departureTime; }
      }

      public DateTime ArrivalTime
      {
         get { return _arrivalTime; }
      }

      public static bool operator ==(Schedule left, Schedule right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(Schedule left, Schedule right)
      {
         return NotEqualOperator(left, right);
      }

      protected override IEnumerable<object> GetAtomicValues()
      {
         foreach (var movement in CarrierMovements)
         {
            yield return movement;
         }
      }

      protected Schedule()
      {         
      }
   }
}