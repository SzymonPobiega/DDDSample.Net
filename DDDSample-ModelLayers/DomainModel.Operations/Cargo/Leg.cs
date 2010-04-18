using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.DomainModel.Potential.Location;
using DDDSample.DomainModel.Potential.Voyage;

namespace DDDSample.DomainModel.Operations.Cargo
{
#pragma warning disable 660,661
   public class Leg : ValueObject
#pragma warning restore 660,661
   {
      private readonly Voyage _voyage;

      private readonly Location _loadLocation;
      private readonly Location _unloadLocation;

      private readonly DateTime _loadDate;
      private readonly DateTime _unloadDate;

      /// <summary>
      /// Creates new leg instance.
      /// </summary>
      /// <param name="voyage">Voyage</param>
      /// <param name="loadLocation">Location where cargo is supposed to be loaded.</param>
      /// <param name="loadDate">Date and time when cargo is supposed to be loaded</param>
      /// <param name="unloadLocation">Location where cargo is supposed to be unloaded.</param>
      /// <param name="unloadDate">Date and time when cargo is supposed to be unloaded.</param>
      public Leg(Voyage voyage, Location loadLocation, DateTime loadDate, Location unloadLocation, DateTime unloadDate)
      {
         _loadLocation = loadLocation;
         _voyage = voyage;
         _unloadDate = unloadDate;
         _unloadLocation = unloadLocation;
         _loadDate = loadDate;
      }

      /// <summary>
      /// Calculates cost of transporting cargo via this leg.
      /// </summary>
      public decimal CalculateCost()
      {
         var movements = _voyage.Schedule.CarrierMovements;
         int firstMovementIndex = GetFirstMovementIndex(movements);
         int lastMovementIndex = GetLastMovementIndex(movements);
         var thisLegMovementCount = lastMovementIndex - firstMovementIndex + 1;

         var thisLegMovements = movements
            .Skip(firstMovementIndex)
            .Take(thisLegMovementCount);

         return thisLegMovements.Sum(x => x.PricePerCargo);
      }

      private int GetLastMovementIndex(IList<CarrierMovement> movements)
      {
         var lastMovement = movements.Single(IsLastMovementOfLeg);
         return movements.IndexOf(lastMovement);
      }

      private int GetFirstMovementIndex(IList<CarrierMovement> movements)
      {
         var firstMovement = movements.Single(IsFirstMovementOfLeg);
         return movements.IndexOf(firstMovement);
      }

      private bool IsFirstMovementOfLeg(CarrierMovement x)
      {
         return x.DepartureTime == LoadDate && x.TransportLeg.DepartureLocation == LoadLocation;
      }

      private bool IsLastMovementOfLeg(CarrierMovement x)
      {
         return x.ArrivalTime == UnloadDate && x.TransportLeg.ArrivalLocation == UnloadLocation;
      }

      /// <summary>
      /// Gets voyage.
      /// </summary>
      public Voyage Voyage
      {
         get { return _voyage; }
      }

      /// <summary>
      /// Gets location where cargo is supposed to be loaded.
      /// </summary>
      public Location LoadLocation
      {
         get { return _loadLocation; }
      }

      /// <summary>
      /// Gets location where cargo is supposed to be unloaded.
      /// </summary>
      public Location UnloadLocation
      {
         get { return _unloadLocation; }
      }

      /// <summary>
      /// Gets date and time when cargo is supposed to be loaded.
      /// </summary>
      public DateTime LoadDate
      {
         get { return _loadDate; }
      }

      /// <summary>
      /// Gets date and time when cargo is supposed to be unloaded.
      /// </summary>
      public DateTime UnloadDate
      {
         get { return _unloadDate; }
      }

      #region Infrastructure
      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return _loadLocation;
         yield return _unloadLocation;
         yield return _loadDate;
         yield return _unloadDate;
         yield return _voyage;
      }

      public static bool operator ==(Leg left, Leg right)
      {
         return EqualOperator(left, right);
      }

      public static bool operator !=(Leg left, Leg right)
      {
         return NotEqualOperator(left, right);
      }

      protected Leg()
      {
      }

      #endregion
   }
}