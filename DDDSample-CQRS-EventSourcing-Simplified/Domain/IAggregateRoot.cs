using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain
{
   public interface IAggregateRoot
   {
      Guid Id { get; set; }
      int Version { get; set; }
      ICollection<object> Events { get; }
      void LoadFromEventStream(IEnumerable<object> events);
   }
}