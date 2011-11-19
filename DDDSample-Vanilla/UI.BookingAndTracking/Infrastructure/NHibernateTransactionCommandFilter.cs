using LeanCommandUnframework;
using NHibernate;
using NHibernate.Context;

namespace DDDSample.UI.BookingAndTracking.Infrastructure
{
    public class NHibernateTransactionCommandFilter : IFilter<object>
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        private ITransaction _transaction;
        private ISession _ambientSession;

        public NHibernateTransactionCommandFilter(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void OnHandling(object command)
        {
            _ambientSession = CurrentSessionContext.Unbind(_sessionFactory);
            _session = _sessionFactory.OpenSession();
            _transaction = _session.BeginTransaction();
            CurrentSessionContext.Bind(_session);
        }
        public void OnHandled(object command, object result)
        {
            CurrentSessionContext.Unbind(_sessionFactory);
            _transaction.Commit();
            _session.Close();
            CurrentSessionContext.Bind(_ambientSession);
        }
    }
}