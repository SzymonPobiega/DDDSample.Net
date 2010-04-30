using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace DDDSample.Domain.Persistence.NHibernate
{
   /// <summary>
   /// Base class for NHibernate based repositories.
   /// </summary>
   public abstract class AbstractRepository
   {
      protected ISessionFactory SessionFactory { get; private set; }

      protected AbstractRepository(ISessionFactory sessionFactory)
      {
         SessionFactory = sessionFactory;
      }

      protected ISession Session
      {
         get { return SessionFactory.GetCurrentSession(); }
      }
   }
}
