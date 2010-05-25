using System.Web.Mvc;
using DDDSample.UI.BookingAndTracking.Models;
using Reporting.Persistence.Raven;

namespace DDDSample.UI.BookingAndTracking.Controllers
{
   public class TrackingController : Controller
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public TrackingController(CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
      }

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
         Reporting.Cargo cargo = _cargoDataAccess.FindByTrackingId(trackingId);
         if (cargo == null)
         {
            ViewData.ModelState.AddModelError("trackingId", "Provided tracking id is invalid.");
            return View();
         }
         return View(new CargoTrackingViewAdapter(cargo));         
      }
   }
}
