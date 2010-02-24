using System;
using System.Linq;
using System.Collections.Generic;

namespace DDDSample.Domain
{
   public interface IBus
   {
      void Publish(object @event);
   }
}