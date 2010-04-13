using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.DomainModel.Operations.Cargo
{
   /// <summary>
   /// Raised after cargo has arrived assigned to route.
   /// </summary>
   public sealed class CargoHasBeenAssignedToRouteEvent : DomainEvent<DomainModel.Operations.Cargo.Cargo>
   {
      private readonly Itinerary _oldItinerary;

      public CargoHasBeenAssignedToRouteEvent(DomainModel.Operations.Cargo.Cargo source, Itinerary oldItinerary) : base(source)
      {
         _oldItinerary = oldItinerary;
      }

      /// <summary>
      /// Gets the route before assigning cargo to a new one.
      /// </summary>
      public Itinerary OldItinerary
      {
         get { return _oldItinerary; }
      }
   }
}