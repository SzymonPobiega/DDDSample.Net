using DDDSample.DomainModel.Potential.Customer;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class CustomerRepository : AbstractRepository, ICustomerRepository
   {
      public CustomerRepository(ISessionFactory sessionFactory) : base(sessionFactory)
      {
      }

      public Customer Find(string userLogin)
      {
         return Session.CreateCriteria(typeof(Customer))
            .Add(Restrictions.Eq("AssociatedLogin", userLogin))
            .UniqueResult<Customer>();
      }      
   }
}