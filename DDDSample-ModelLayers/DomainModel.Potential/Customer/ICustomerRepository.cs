using System;
using System.Collections.Generic;

namespace DDDSample.DomainModel.Potential.Customer
{
   public interface ICustomerRepository
   {
      Customer Find(string userLogin);
      IList<Customer> FindAll();
   }
}