using DDDSample.DomainModel.Policy.Commitments;
using DDDSample.DomainModel.Policy.Routing;
using DDDSample.DomainModel.Potential.Customer;
using NHibernate;

namespace DDDSample.DomainModel.Persistence
{
   public static class SampleCustomers
   {
      public static readonly Customer C1 = new Customer("C1", "c1");
      public static readonly Customer C2 = new Customer("C2", "c2");

      public static void CreateCustomers(ISession session)
      {
         session.Save(C1);
         session.Save(new CustomerAgreement(C1, new FastestRoutingPolicy()));

         session.Save(C2);
         session.Save(new CustomerAgreement(C2, new CheapestRoutingPolicy()));
      }
   }
}