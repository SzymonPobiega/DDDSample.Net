using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
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
            Configuration cfg = new Configuration().Configure();
            new SchemaExport(cfg).Execute(false, true, false);

            ISessionFactory sessionFactory = cfg.BuildSessionFactory();
            using (ISession session = sessionFactory.OpenSession())
            {
                SampleLocations.CreateLocations(session);
                SampleTransportLegs.CreateTransportLegs(session);
                SampleVoyages.CreateVoyages(session);

                session.Flush();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
