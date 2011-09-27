using System;
using LeanCommandUnframework;
using NHibernate;

namespace Tests.Integration
{
    public class TransactionCommandFilter : IFilter<object>
    {
        private ITransaction _transaction;
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;

        public TransactionCommandFilter(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void OnHandling(object command)
        {
            _session = _sessionFactory.GetCurrentSession();
            _transaction = _session.BeginTransaction();
        }

        public void OnHandled(object command, object result)
        {
            _session.Flush();
            _transaction.Commit();
        }
    }
}