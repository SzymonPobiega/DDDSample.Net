using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain
{
   public interface IAggregateRoot
   {
      string Id { get; set; }
      int OriginalVersion { get; set; }
      int CurrentVersion { get; set; }
      ICollection<object> Events { get; }
      void LoadFromEventStream(IEnumerable<object> events);
   }
}