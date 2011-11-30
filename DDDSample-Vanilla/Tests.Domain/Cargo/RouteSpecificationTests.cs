using System;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace DDDSample.Domain.Tests.Cargo
{
    [TestFixture]
    public class RouteSpecificationTests
    {
        protected static readonly Location.Location Krakow = new Location.Location(new UnLocode("PLKRK"), "Krakow");
        protected static readonly Location.Location Warszawa = new Location.Location(new UnLocode("PLWAW"), "Warszawa");
        protected static readonly Location.Location Wroclaw = new Location.Location(new UnLocode("PLWRC"), "Wroclaw");

        public static DateTime ArrivalDeadline = new DateTime(2011, 12, 24);
        
        [Test]
        public void It_is_satisfied_if_origin_and_destination_match_and_deadline_is_not_exceeded()
        {
            var specification = new RouteSpecification(Krakow, Wroclaw, ArrivalDeadline);

            var itinerary = new Itinerary(new[]
                                              {
                                                  new Leg(null, Krakow, new DateTime(2011, 12, 1), Warszawa,
                                                          new DateTime(2011, 12, 2)),
                                                  new Leg(null, Warszawa, new DateTime(2011, 12, 13), Wroclaw,
                                                          ArrivalDeadline)
                                              });
            Assert.IsTrue(specification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void It_is_not_satisfied_if_arrival_deadline_is_exceeded()
        {
            var specification = new RouteSpecification(Krakow, Wroclaw, ArrivalDeadline);

            var itinerary = new Itinerary(new[]
                                              {
                                                  new Leg(null, Krakow, new DateTime(2011, 12, 1), Warszawa,
                                                          new DateTime(2011, 12, 2)),
                                                  new Leg(null, Warszawa, new DateTime(2011, 12, 13), Wroclaw,
                                                          new DateTime(2011, 12, 25))
                                              });
            Assert.IsFalse(specification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void It_is_not_satisfied_if_origin_does_not_match()
        {
            var specification = new RouteSpecification(Krakow, Wroclaw, ArrivalDeadline);

            var itinerary = new Itinerary(new[]
                                              {
                                                  new Leg(null, Warszawa, new DateTime(2011, 12, 13), Wroclaw,
                                                          new DateTime(2011, 12, 15))
                                              });
            Assert.IsFalse(specification.IsSatisfiedBy(itinerary));
        }

        [Test]
        public void It_is_not_satisfied_if_destination_does_not_match()
        {
            var specification = new RouteSpecification(Krakow, Wroclaw, ArrivalDeadline);

            var itinerary = new Itinerary(new[]
                                              {
                                                  new Leg(null, Krakow, new DateTime(2011, 12, 1), Warszawa,
                                                          new DateTime(2011, 12, 2))
                                              });
            Assert.IsFalse(specification.IsSatisfiedBy(itinerary));
        }
    }
}
// ReSharper restore InconsistentNaming
