using System;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class Event
   {
      public int SequenceNumber { get; set; }
      public int Version { get; set; }
      public Guid EntityId { get; set; }
      public object Data { get; set; }
      public bool IsSnapshot { get; set; }
   }
}