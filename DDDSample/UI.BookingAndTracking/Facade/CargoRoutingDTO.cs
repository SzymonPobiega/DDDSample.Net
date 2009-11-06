using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Data Transfer Object for displaying cargo information in context of assigning route.
   /// </summary>
   public class CargoRoutingDTO
   {
      private readonly string _trackingId;
      private readonly string _origin;
      private readonly string _destination;
      private readonly DateTime _arrivalDeadline;
      private readonly bool _misrouted;
      private readonly IList<LegDTO> _legs;

      public CargoRoutingDTO(string trackingId, string origin, string destination, DateTime arrivalDeadline, bool misrouted, IList<LegDTO> legs)
      {
         _trackingId = trackingId;
         _misrouted = misrouted;
         _legs = legs;
         _arrivalDeadline = arrivalDeadline;
         _destination = destination;
         _origin = origin;
      }

      public bool Misrouted
      {
         get { return _misrouted; }
      }

      public bool IsRouted
      {
         get { return _legs.Count > 0; }
      }

      public IList<LegDTO> Legs
      {
         get { return _legs; }
      }

      public DateTime ArrivalDeadline
      {
         get { return _arrivalDeadline; }
      }

      public string Destination
      {
         get { return _destination; }
      }

      public string Origin
      {
         get { return _origin; }
      }

      public string TrackingId
      {
         get { return _trackingId; }
      }
   }
}