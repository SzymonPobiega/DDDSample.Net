using System;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class AggregateRootMetadata
   {
      public string Id { get; set; }
      public int RecentSnapshotVersion { get; set; }
   }
}