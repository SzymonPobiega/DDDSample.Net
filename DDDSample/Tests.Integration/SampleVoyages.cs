using System;
using System.Collections.Generic;
using System.Linq;
using DDDSample.DomainModel.Potential.Voyage;
using NHibernate;

namespace Tests.Integration
{
   public static class SampleVoyages
   {
      public static DateTime ReferenceDate = DateTime.Now;

      public static readonly Voyage V100 =
         Voyage("V100",
                Movement(SampleTransportLegs.FromHongkongToTokyo, 0, 2),
                Movement(SampleTransportLegs.FromTokyoToNewYork, 3, 6));

      public static readonly Voyage V200 =
         Voyage("V200",
                Movement(SampleTransportLegs.FromTokyoToNewYork, 3, 5),
                Movement(SampleTransportLegs.FromNewYorkToChicago, 7, 11),
                Movement(SampleTransportLegs.FromChicagoToStockholm, 11, 13));

      public static readonly Voyage V300 =
         Voyage("V300",
                Movement(SampleTransportLegs.FromTokyoToRotterdam, 5, 8),
                Movement(SampleTransportLegs.FromRotterdamToHamburg, 8, 9),
                Movement(SampleTransportLegs.FromHamburgToMelbourne, 11, 15),
                Movement(SampleTransportLegs.FromMelbourneToTokyo, 16, 18));

      public static readonly Voyage V400 =
         Voyage("V400",
                Movement(SampleTransportLegs.FromHamburgToStockholm, 11, 12),
                Movement(SampleTransportLegs.FromStockholmToHelsinki, 12, 13),
                Movement(SampleTransportLegs.FromHelsinkiToHamburg, 17, 19));

      public static void CreateVoyages(ISession session)
      {
         session.Save(V100);
         session.Save(V200);
         session.Save(V300);
         session.Save(V400);
      }

      private static Voyage Voyage(string number, params CarrierMovement[] movements)
      {
         return new Voyage(new VoyageNumber(number), new Schedule(movements.ToList()));
      }

      private static CarrierMovement Movement(TransportLeg transportLeg, int departureOffset, int arrivalOffset)
      {
         return new CarrierMovement(transportLeg, ReferenceDate.AddDays(departureOffset), ReferenceDate.AddDays(arrivalOffset));
      }
   }
}