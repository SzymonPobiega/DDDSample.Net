namespace DDDSample.DomainModel.Potential.Voyage
{
   public class TransportLeg
   {
      /// <summary>
      /// Gets departure location.
      /// </summary>
      public virtual Location.Location DepartureLocation { get; protected set; }
      /// <summary>
      /// Gets arrival location.
      /// </summary>
      public virtual Location.Location ArrivalLocation { get; protected set; }

      public TransportLeg(Location.Location departureLocation, 
                          Location.Location arrivalLocation)
      {
         DepartureLocation = departureLocation;
         ArrivalLocation = arrivalLocation;
      }

      protected TransportLeg()
      {         
      }
   }
}