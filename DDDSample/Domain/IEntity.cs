using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain
{
   /// <summary>
   /// Base interface for all entities in the model.
   /// </summary>
   /// <typeparam name="T">Entity type implementing the interface.</typeparam>
   /// <example>Cargo : IEntity&gt;Cargo&lt;</example>
   public interface IEntity<T>
      where T : IEntity<T>
   {
      /// <summary>
      /// Compares entities by their identities (business keys).
      /// </summary>
      /// <param name="other">The other entity.</param>
      /// <returns>True when entities have the same identity.</returns>
      bool HasSameIdentityAs(T other);
   }
}
