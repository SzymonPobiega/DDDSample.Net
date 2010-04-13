using System;

namespace DDDSample.DomainModel.Potential.Customer
{
   public class Customer
   {
      /// <summary>
      /// Gets the name of the customer.
      /// </summary>
      public virtual string Name { get; protected set; }

      /// <summary>
      /// Gets the login associated with this customer.
      /// </summary>
      public virtual string AssociatedLogin { get; protected set; }

      public Customer(string name, string associatedLogin)
      {
         Name = name;
         AssociatedLogin = associatedLogin;
      }

      protected Customer()
      {
      }
   }
}