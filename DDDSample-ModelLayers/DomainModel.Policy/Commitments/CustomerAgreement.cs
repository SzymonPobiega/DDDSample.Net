using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Policy.Routing;
using DDDSample.DomainModel.Potential.Customer;

namespace DDDSample.DomainModel.Policy.Commitments
{
   public class CustomerAgreement
   {      
      /// <summary>
      /// Gets the routing policy this agreement associates with its subject.
      /// </summary>
      public virtual IRoutingPolicy RoutingPolicy { get; protected set; }

      /// <summary>
      /// Gets the subject of this agreement.
      /// </summary>
      public virtual Customer Subject { get; protected set; }

      /// <summary>
      /// Creates an empty customer agreement (with null policies)
      /// for use with customers who didn't signed an agreement.
      /// </summary>
      /// <param name="subject">Customer without agreement.</param>
      /// <returns></returns>
      public static CustomerAgreement CreateEmpty(Customer subject)
      {
         return new CustomerAgreement(subject, new NullRoutingPolicy());
      }

      /// <summary>
      /// Creates new customer agreement.
      /// </summary>
      /// <param name="subject"></param>
      /// <param name="routingPolicy"></param>
      public CustomerAgreement(Customer subject, IRoutingPolicy routingPolicy)
      {
         Subject = subject;
         RoutingPolicy = routingPolicy;
      }

      protected CustomerAgreement()
      {         
      }
   }
}