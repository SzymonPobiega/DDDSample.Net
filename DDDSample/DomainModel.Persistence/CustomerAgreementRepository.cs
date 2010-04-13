using DDDSample.DomainModel.Policy.Commitments;
using DDDSample.DomainModel.Potential.Customer;
using NHibernate;
using NHibernate.Criterion;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class CustomerAgreementRepository : AbstractRepository, ICustomerAgreementRepository
   {
      public CustomerAgreementRepository(ISessionFactory sessionFactory) 
         : base(sessionFactory)
      {
      }

      public CustomerAgreement Find(Customer customer)
      {
         var result = Session.CreateCriteria(typeof (CustomerAgreement))
            .Add(Restrictions.Eq("Subject", customer))
            .UniqueResult<CustomerAgreement>();         
         return result ?? CustomerAgreement.CreateEmpty(customer);
      }
   }
}