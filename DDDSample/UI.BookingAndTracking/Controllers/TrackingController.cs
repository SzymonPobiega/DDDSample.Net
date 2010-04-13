using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.UI.BookingAndTracking.Models;

namespace DDDSample.UI.BookingAndTracking.Controllers
{
   public class TrackingController : Controller
   {
      private readonly ICargoRepository _cargoRepository;
      private readonly IHandlingEventRepository _handlingEventRepository;

      public TrackingController(ICargoRepository cargoRepository, IHandlingEventRepository handlingEventRepository)
      {
         _cargoRepository = cargoRepository;
         _handlingEventRepository = handlingEventRepository;
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
         Cargo cargo = _cargoRepository.Find(new TrackingId(trackingId));
         if (cargo == null)
         {
            ViewData.ModelState.AddModelError("trackingId", "Provided tracking id is invalid.");
            return View();
         }
         HandlingHistory history = _handlingEventRepository.LookupHandlingHistoryOfCargo(new TrackingId(trackingId));
         return View(new CargoTrackingViewAdapter(cargo, history));         
      }
   }
}
