using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDDSample.Domain.Location;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DDDSample.UI.BookingAndTracking.Controllers
{
   [HandleError]
   public class DatabaseAdminController : Controller
   {      
      public ActionResult Reset()
      {
         Configuration cfg = MvcApplication.InitializeNHibernate(x => x);
         new SchemaExport(cfg).Execute(false, true, false);         

         ISessionFactory sessionFactory = cfg.BuildSessionFactory();
         using (ISession session = sessionFactory.OpenSession())
         {
            session.Save(new Location(new UnLocode("CNHKG"), "Hongkong"));
            session.Save(new Location(new UnLocode("AUMEL"), "Melbourne"));
            session.Save(new Location(new UnLocode("SESTO"), "Stockholm"));
            session.Save(new Location(new UnLocode("FIHEL"), "Helsinki"));
            session.Save(new Location(new UnLocode("USCHI"), "Chicago"));
            session.Save(new Location(new UnLocode("JNTKO"), "Tokyo"));
            session.Save(new Location(new UnLocode("DEHAM"), "Hamburg"));
            session.Save(new Location(new UnLocode("CNSHA"), "Shanghai"));
            session.Save(new Location(new UnLocode("NLRTM"), "Rotterdam"));
            session.Save(new Location(new UnLocode("SEGOT"), "Göteborg"));
            session.Save(new Location(new UnLocode("CNHGH"), "Hangzhou"));
            session.Save(new Location(new UnLocode("USNYC"), "New York"));
            session.Save(new Location(new UnLocode("USDAL"), "Dallas"));
            session.Flush();
         }

         return RedirectToAction("Index", "Home");         
      }
   }
}
