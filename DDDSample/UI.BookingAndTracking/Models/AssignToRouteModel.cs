using System;
using System.Linq;
using System.Collections.Generic;
using UI.BookingAndTracking.Facade;

namespace UI.BookingAndTracking.Models
{
   public class AssignToRouteModel
   {
      private readonly CargoRoutingDTO _cargo;      
      private readonly IList<RouteCandidateDTO> _routeCandidates;

      public AssignToRouteModel(CargoRoutingDTO cargo, IList<RouteCandidateDTO> routeCandidates)
      {
         _routeCandidates = routeCandidates;
         _cargo = cargo;
      }

      public CargoRoutingDTO Cargo
      {
         get { return _cargo; }
      }

      public IList<RouteCandidateDTO> RouteCandidates
      {
         get { return _routeCandidates; }
      }
   }
}