using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDSample.Domain.Location
{
   /// <summary>
   /// A location is our model is stops on a journey, such as cargo
   /// origin or destination, or carrier movement endpoints.
   /// 
   /// It is uniquely identified by a UN Locode.
   /// </summary>
   public class Location : IAggregateRoot<Location>
   {
      public virtual Guid Id { get; protected set; }
      /// <summary>
      /// Gets the <see cref="UnLocode"/> for this location.
      /// </summary>
      public virtual UnLocode UnLocode { get; protected set; }

      /// <summary>
      /// Gets the name of this location, e.g. Krakow.
      /// </summary>
      public virtual string Name { get; protected set; }

      /// <summary>
      /// Returns an instance indicating an unknown location.
      /// </summary>
      public static Location Unknown
      {
         get { return new Location(new UnLocode("XXXXX"), "Unknown location");}
      }

      /// <summary>
      /// Creates new location.
      /// </summary>
      /// <param name="locode"><see cref="UnLocode"/> for this location.</param>
      /// <param name="name">Name.</param>
      public Location(UnLocode locode, string name)
      {
         UnLocode = locode;
         Name = name;
      }      

      /// <summary>
      /// For NHibernate.
      /// </summary>
      protected Location()
      {
      }

      public override string ToString()
      {
         return Name + " [" + UnLocode + "]";
      }
   }
}
