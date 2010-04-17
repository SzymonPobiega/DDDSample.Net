using DDDSample.DomainModel.Potential.Voyage;
using NHibernate;

namespace DDDSample.DomainModel.Persistence
{
   public static class SampleTransportLegs
   {
      public static readonly TransportLeg FromHongkongToTokyo
         = new TransportLeg(SampleLocations.Hongkong, SampleLocations.Tokyo);

      public static readonly TransportLeg FromTokyoToNewYork
         = new TransportLeg(SampleLocations.Tokyo, SampleLocations.NewYork);

      public static readonly TransportLeg FromNewYorkToChicago
         = new TransportLeg(SampleLocations.NewYork, SampleLocations.Chicago);

      public static readonly TransportLeg FromChicagoToStockholm
         = new TransportLeg(SampleLocations.Chicago, SampleLocations.Stockholm);

      public static readonly TransportLeg FromTokyoToRotterdam
         = new TransportLeg(SampleLocations.Tokyo, SampleLocations.Rotterdam);

      public static readonly TransportLeg FromRotterdamToHamburg
         = new TransportLeg(SampleLocations.Rotterdam, SampleLocations.Hamburg);

      public static readonly TransportLeg FromHamburgToMelbourne
         = new TransportLeg(SampleLocations.Hamburg, SampleLocations.Melbourne);

      public static readonly TransportLeg FromMelbourneToTokyo
         = new TransportLeg(SampleLocations.Melbourne, SampleLocations.Tokyo);

      public static readonly TransportLeg FromHamburgToStockholm
         = new TransportLeg(SampleLocations.Hamburg, SampleLocations.Stockholm);

      public static readonly TransportLeg FromStockholmToHelsinki
         = new TransportLeg(SampleLocations.Stockholm, SampleLocations.Helsinki);

      public static readonly TransportLeg FromHelsinkiToHamburg
         = new TransportLeg(SampleLocations.Helsinki, SampleLocations.Hamburg);

      public static void CreateTransportLegs(ISession session)
      {
         session.Save(FromHongkongToTokyo);
         session.Save(FromTokyoToNewYork);
         session.Save(FromNewYorkToChicago);
         session.Save(FromChicagoToStockholm);
         session.Save(FromTokyoToRotterdam);
         session.Save(FromRotterdamToHamburg);
         session.Save(FromHamburgToMelbourne);
         session.Save(FromMelbourneToTokyo);
         session.Save(FromHamburgToStockholm);
         session.Save(FromStockholmToHelsinki);
         session.Save(FromHelsinkiToHamburg);
      }
   }
}