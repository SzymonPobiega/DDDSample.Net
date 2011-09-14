using NServiceBus;
using NServiceBus.ObjectBuilder;
using NServiceBus.SagaPersisters.NHibernate;

namespace DDDSample.UI.BookingAndTracking
{
    public static class NHibernateMessageModuleConfig
    {
        public static Configure NHibernateMessageModule(this Configure configure)
        {
            configure.Configurer.ConfigureComponent<NHibernateMessageModule>(ComponentCallModelEnum.Singlecall);
            return configure;
        }
    }
}