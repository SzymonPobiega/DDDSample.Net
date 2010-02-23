using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DDDSample.Domain.Cargo;
using DDDSample.Reporting;
using DDDSample.UI.BookingAndTracking.Models;

namespace DDDSample.UI.BookingAndTracking.Controllers
{
   public class TrackingController : Controller
   {      
      [AcceptVerbs(HttpVerbs.Get)]
      [ActionName("Track")]
      public ActionResult TrackGet(string trackingId)
      {
         if (string.IsNullOrEmpty(trackingId))
         {            
            return View();
         }
         return Track(trackingId);
      }

      [AcceptVerbs(HttpVerbs.Post)]
      [ActionName("Track")]
      public ActionResult TrackPost(string trackingId)
      {
         if (string.IsNullOrEmpty(trackingId))
         {
            ViewData.ModelState.AddModelError("trackingId", "Tracking id must by specified.");
            return View();
         }
         return Track(trackingId);         
      }  
    
      public ActionResult Track(string trackingId)
      {
         ReportingDataContext context = new ReportingDataContext();
         Reporting.Cargo cargo = context.Cargos.FirstOrDefault(x => x.TrackingId == trackingId);
         if (cargo == null)
         {
            ViewData.ModelState.AddModelError("trackingId", "Provided tracking id is invalid.");
            return View();
         }
         return View(new CargoTrackingViewAdapter(cargo));         
      }
   }
}
