using System.Collections.Generic;
using Autofac;
using DDDSample.UI.BookingAndTracking.Infrastructure;
using NHibernate.Cfg;

namespace DDDSample.UI.BookingAndTracking.Composition
{
    public class NHibernateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var cfg = new Configuration().Configure();
            cfg.AddProperties(new Dictionary<string, string>
                                  {
                                      {"current_session_context_class", "NHibernate.Context.WebSessionContext"}
                                  });
            var sessionFactory = cfg.BuildSessionFactory();
            builder.RegisterInstance(sessionFactory);
            builder.RegisterType<NHibernateAmbientSessionManager>().AsSelf();
        }
    }
}