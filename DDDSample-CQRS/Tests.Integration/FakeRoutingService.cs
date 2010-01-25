using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;

namespace Tests.Integration
{
   public class FakeRoutingService : IRoutingService
   {           
      public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
      {
         if (routeSpecification.Destination == ScenarioTest.STOCKHOLM) {
            // Hongkong - NYC - Chicago - Stockholm, initial routing
            return new List<Itinerary>{
                                         new Itinerary(new List<Leg>{
                                                                       new Leg(ScenarioTest.HONGKONG, new DateTime(2009,3,03), ScenarioTest.NEWYORK, new DateTime(2009,3,9)),
                                                                       new Leg(ScenarioTest.NEWYORK, new DateTime(2009,3,10), ScenarioTest.CHICAGO, new DateTime(2009,3,14)),
                                                                       new Leg(ScenarioTest.CHICAGO, new DateTime(2009,3,7), ScenarioTest.STOCKHOLM, new DateTime(2009,3,11))
                                                                    })
                                      };
         } else {
            // Tokyo - Hamburg - Stockholm, rerouting misdirected cargo from Tokyo 
            return new List<Itinerary>{
                                         new Itinerary(new List<Leg>{
                                                                       new Leg(ScenarioTest.HONGKONG, new DateTime(2009,3,8), ScenarioTest.HAMBURG, new DateTime(2009,3,12)),
                                                                       new Leg(ScenarioTest.HAMBURG, new DateTime(2009,3,14), ScenarioTest.GOETEBORG, new DateTime(2009,3,15))
                                                                    })
                                      };
         }
      }
   }
}