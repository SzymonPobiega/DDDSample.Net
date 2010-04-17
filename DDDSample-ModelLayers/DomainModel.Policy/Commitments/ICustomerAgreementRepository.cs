using DDDSample.DomainModel.Potential.Customer;

namespace DDDSample.DomainModel.Policy.Commitments
{
   public interface ICustomerAgreementRepository
   {
      CustomerAgreement Find(Customer customer);
   }
}