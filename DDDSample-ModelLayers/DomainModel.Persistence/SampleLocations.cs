using DDDSample.DomainModel.Potential.Location;
using NHibernate;

namespace DDDSample.DomainModel.Persistence
{
   public static class SampleLocations
   {
      public static readonly Location Chicago = new Location(new UnLocode("USCHI"), "Chicago");
      public static readonly Location Dallas = new Location(new UnLocode("USDAL"), "Dallas");
      public static readonly Location Goeteborg = new Location(new UnLocode("SEGOT"), "Göteborg");
      public static readonly Location Hamburg = new Location(new UnLocode("DEHAM"), "Hamburg");
      public static readonly Location Hangzhou = new Location(new UnLocode("CNHGH"), "Hangzhou");
      public static readonly Location Helsinki = new Location(new UnLocode("FIHEL"), "Helsinki");
      public static readonly Location Hongkong = new Location(new UnLocode("CNHKG"), "Hongkong");
      public static readonly Location Melbourne = new Location(new UnLocode("AUMEL"), "Melbourne");
      public static readonly Location NewYork = new Location(new UnLocode("USNYC"), "New York");
      public static readonly Location Rotterdam = new Location(new UnLocode("NLRTM"), "Rotterdam");
      public static readonly Location Shanghai = new Location(new UnLocode("CNSHA"), "Shanghai");
      public static readonly Location Stockholm = new Location(new UnLocode("SESTO"), "Stockholm");
      public static readonly Location Tokyo = new Location(new UnLocode("JNTKO"), "Tokyo");

      public static void CreateLocations(ISession session)
      {
         session.Save(Hongkong);
         session.Save(Melbourne);
         session.Save(Stockholm);
         session.Save(Helsinki);
         session.Save(Chicago);
         session.Save(Tokyo);
         session.Save(Hamburg);
         session.Save(Shanghai);
         session.Save(Rotterdam);
         session.Save(Goeteborg);
         session.Save(Hangzhou);
         session.Save(NewYork);
         session.Save(Dallas);
      }
   }
}