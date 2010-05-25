using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDDSample.Domain.Location;
using Raven.Client;

namespace DDDSample.UI.BookingAndTracking.Controllers
{
   [HandleError]
   public class DatabaseAdminController : Controller
   {
      private readonly IDocumentStore _documentStore;

      public DatabaseAdminController(IDocumentStore documentStore)
      {
         _documentStore = documentStore;
      }

      public ActionResult Reset()
      {
         using (var session = _documentStore.OpenSession())
         {
            session.Store(new Location(new UnLocode("CNHKG"), "Hongkong"));
            session.Store(new Location(new UnLocode("AUMEL"), "Melbourne"));
            session.Store(new Location(new UnLocode("SESTO"), "Stockholm"));
            session.Store(new Location(new UnLocode("FIHEL"), "Helsinki"));
            session.Store(new Location(new UnLocode("USCHI"), "Chicago"));
            session.Store(new Location(new UnLocode("JNTKO"), "Tokyo"));
            session.Store(new Location(new UnLocode("DEHAM"), "Hamburg"));
            session.Store(new Location(new UnLocode("CNSHA"), "Shanghai"));
            session.Store(new Location(new UnLocode("NLRTM"), "Rotterdam"));
            session.Store(new Location(new UnLocode("SEGOT"), "Göteborg"));
            session.Store(new Location(new UnLocode("CNHGH"), "Hangzhou"));
            session.Store(new Location(new UnLocode("USNYC"), "New York"));
            session.Store(new Location(new UnLocode("USDAL"), "Dallas"));            

            session.SaveChanges();            
         }         

         return RedirectToAction("Index", "Home");         
      }
   }
}
