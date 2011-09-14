using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using NHibernate.Cfg;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class NHibernateModule : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var cfg = new Configuration().Configure();
            cfg.AddProperties(new Dictionary<string, string>
                                  {
                                      {"current_session_context_class", "NHibernate.Context.WebSessionContext"}
                                  });
            var sessionFactory = cfg.BuildSessionFactory();
            Container.RegisterInstance(sessionFactory);
        }
    }
}