using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class IntraAggregateEntityReferenceConvention : IReferenceConvention, IReferenceConventionAcceptance
   {            
      public void Apply(IManyToOneInstance instance)
      {         
         instance.Cascade.All();
      }

      public void Accept(IAcceptanceCriteria<IManyToOneInspector> criteria)
      {
         criteria.Expect(x => IsIntraAggregateReference(x));
      }

      private static bool IsIntraAggregateReference(IManyToOneInspector manyToOne)
      {
         var thisAggregate = GetAggregateType(manyToOne.Property.DeclaringType);
         var referredAggregate = GetAggregateType(manyToOne.Class.GetUnderlyingSystemType());

         return thisAggregate == referredAggregate;
      }

      private static Type GetAggregateType(Type entityType)
      {
         var entityInterfaceType = GetEntityInterfaceType(entityType);
         return entityInterfaceType.GetGenericArguments()[0];
      }

      private static Type GetEntityInterfaceType(Type entityType)
      {
         return entityType.GetInterfaces()
            .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (IAggregateMember<>));
      }
   }
}