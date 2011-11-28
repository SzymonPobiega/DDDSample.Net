using DDDSample.Domain.Location;
using NHibernate;

namespace DDDSample.Domain.Persistence.NHibernate
{
    public static class SampleLocations
    {
        public static readonly Location.Location Chicago = new Location.Location(new UnLocode("USCHI"), "Chicago");
        public static readonly Location.Location Dallas = new Location.Location(new UnLocode("USDAL"), "Dallas");
        public static readonly Location.Location Goeteborg = new Location.Location(new UnLocode("SEGOT"), "Göteborg");
        public static readonly Location.Location Hamburg = new Location.Location(new UnLocode("DEHAM"), "Hamburg");
        public static readonly Location.Location Hangzhou = new Location.Location(new UnLocode("CNHGH"), "Hangzhou");
        public static readonly Location.Location Helsinki = new Location.Location(new UnLocode("FIHEL"), "Helsinki");
        public static readonly Location.Location Hongkong = new Location.Location(new UnLocode("CNHKG"), "Hongkong");
        public static readonly Location.Location Melbourne = new Location.Location(new UnLocode("AUMEL"), "Melbourne");
        public static readonly Location.Location NewYork = new Location.Location(new UnLocode("USNYC"), "New York");
        public static readonly Location.Location Rotterdam = new Location.Location(new UnLocode("NLRTM"), "Rotterdam");
        public static readonly Location.Location Shanghai = new Location.Location(new UnLocode("CNSHA"), "Shanghai");
        public static readonly Location.Location Stockholm = new Location.Location(new UnLocode("SESTO"), "Stockholm");
        public static readonly Location.Location Tokyo = new Location.Location(new UnLocode("JNTKO"), "Tokyo");

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