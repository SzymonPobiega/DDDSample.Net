//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace DDDSample.Domain.Voyage
//{
//   public class CarrierMovement : ValueObject
//   {
//      private Location.Location _departureLocation;
//      private Location.Location _arrivalLocation;
//      private DateTime _departureTime;
//      private DateTime _arrivalTime;

//      public CarrierMovement(Location.Location departureLocation, Location.Location arrivalLocation, DateTime departureTime, DateTime arrivalTime)
//      {
//         _departureLocation = departureLocation;
//         _arrivalTime = arrivalTime;
//         _departureTime = departureTime;
//         _arrivalLocation = arrivalLocation;
//      }

//      public static bool operator ==(CarrierMovement left, CarrierMovement right)
//      {
//         return EqualOperator(left, right);
//      }

//      public static bool operator !=(CarrierMovement left, CarrierMovement right)
//      {
//         return NotEqualOperator(left, right);
//      }

//      protected override IEnumerable<object> GetAtomicValues()
//      {
//         throw new NotImplementedException();
//      }
      
//      protected CarrierMovement()
//      {         
//      }
//   }
//}