using System;
using System.Linq;

namespace DDDSample.DomainModel.Potential.Voyage
{
   /// <summary>
   /// A voyage
   /// </summary>
   public class Voyage
   {
      /// <summary>
      /// Gets unique id of this voyage.
      /// </summary>
      public virtual Guid Id { get; protected set; }      

      /// <summary>
      /// Gets (non-unique) voyage number associated with this voyage.
      /// </summary>
      public virtual VoyageNumber Number { get; protected set; }

      /// <summary>
      /// Gets the schedule associated with this voyage.
      /// </summary>
      public virtual Schedule Schedule { get; protected set; }

      /// <summary>
      /// Creates new Voyage object.
      /// </summary>
      /// <param name="number">Voyage number of this voyage.</param>
      /// <param name="schedule">Schedule of this voyage</param>
      public Voyage(VoyageNumber number, Schedule schedule)
      {
         Number = number;
         Schedule = schedule;
      }

      protected Voyage()
      {
      }
   }
}