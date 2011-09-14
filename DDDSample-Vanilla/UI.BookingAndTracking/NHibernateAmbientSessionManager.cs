using NHibernate;
using NHibernate.Context;

namespace DDDSample.UI.BookingAndTracking
{
    /// <summary>
    /// Manages the ambient session.
    /// </summary>
    public class NHibernateAmbientSessionManager
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateAmbientSessionManager(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        /// Creates a new session and binds it to the current context.
        /// </summary>
        public void CreateAndBind()
        {
            CurrentSessionContext.Bind(_sessionFactory.OpenSession());
        }

        /// <summary>
        /// Unbinds a session from the current context and disposes it.
        /// </summary>
        public void UnbindAndDispose()
        {
            var session = CurrentSessionContext.Unbind(_sessionFactory);
            if (session != null)
            {
                session.Dispose();
            }
        }
    }
}