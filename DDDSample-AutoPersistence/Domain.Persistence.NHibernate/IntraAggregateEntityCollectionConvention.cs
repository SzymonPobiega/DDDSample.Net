using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class IntraAggregateEntityCollectionConvention : IHasManyConvention
   {      
      public void Apply(IOneToManyCollectionInstance instance)
      {
         instance.Cascade.AllDeleteOrphan();
      }      
   }
}
