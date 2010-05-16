using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.UI.BookingAndTracking.Facade;

namespace DDDSample.UI.BookingAndTracking.Models
{
   /// <summary>
   /// A model for Assign to Route view.
   /// </summary>
   public class AssignToRouteModel
   {
      private readonly Reporting.Cargo _cargo;      
      private readonly IList<RouteCandidateDTO> _routeCandidates;

      public AssignToRouteModel(Reporting.Cargo cargo, IList<RouteCandidateDTO> routeCandidates)
      {
         _routeCandidates = routeCandidates;
         _cargo = cargo;
      }

      public Reporting.Cargo Cargo
      {
         get { return _cargo; }
      }

      public IList<RouteCandidateDTO> RouteCandidates
      {
         get { return _routeCandidates; }
      }
   }
}