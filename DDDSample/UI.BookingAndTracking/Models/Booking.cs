using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.BookingAndTracking.Models
{
   public class Booking
   {
      private readonly IList<SelectListItem> _locations;

      public Booking(IList<SelectListItem> locations)
      {         
         _locations = locations;
         _locations.First().Selected = true;
      }

      public IList<SelectListItem> Locations
      {
         get { return _locations; }
      }
   }
}
