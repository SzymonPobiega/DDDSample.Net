using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDDSample.Domain.Location;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace UI.BookingAndTracking.Controllers
{
   [HandleError]
   public class DatabaseAdminController : Controller
   {      
      public ActionResult Reset()
      {
         Configuration cfg = new Configuration().Configure();
         new SchemaExport(cfg).Execute(false, true, false);

         ISessionFactory sessionFactory = cfg.BuildSessionFactory();
         using (ISession session = sessionFactory.OpenSession())
         {
            session.Save(new Location(new UnLocode("PLKRK"), "Krakow"));
            session.Save(new Location(new UnLocode("PLWAW"), "Warszawa"));
            session.Save(new Location(new UnLocode("PLWRC"), "Wroclaw"));
            session.Flush();
         }

         return RedirectToAction("Index", "Home");         
      }
   }
}
