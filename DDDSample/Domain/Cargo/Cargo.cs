using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Cargo
{
   public class Cargo : IEntity<Cargo>
   {


      public bool HasSameIdentityAs(Cargo other)
      {
         throw new NotImplementedException();
      }
   }
}
