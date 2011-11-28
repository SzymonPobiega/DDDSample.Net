//using System;
//using System.Linq;
//using System.Collections.Generic;
//using DDDSample.Domain;
//using DDDSample.Domain.Cargo;

//namespace Tests.Integration
//{
//   public class FakeRoutingService : IRoutingService
//   {           
//      public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
//      {
//         if (routeSpecification.Origin == ScenarioTest.Hongkong) {
//            // Hongkong - NYC - Chicago - Stockholm, initial routing
//            return new List<Itinerary>{
//                                         new Itinerary(new List<Leg>{
//                                                                       new Leg(ScenarioTest.Hongkong, new DateTime(2009,3,03), ScenarioTest.NewYork, new DateTime(2009,3,9)),
//                                                                       new Leg(ScenarioTest.NewYork, new DateTime(2009,3,10), ScenarioTest.Chicago, new DateTime(2009,3,14)),
//                                                                       new Leg(ScenarioTest.Chicago, new DateTime(2009,3,7), ScenarioTest.Stockholm, new DateTime(2009,3,11))
//                                                                    })
//                                      };
//         } else {
//            // Tokyo - Hamburg - Stockholm, rerouting misdirected cargo from Tokyo 
//            return new List<Itinerary>{
//                                         new Itinerary(new List<Leg>{
//                                                                       new Leg(ScenarioTest.Tokyo, new DateTime(2009,3,8), ScenarioTest.Hamburg, new DateTime(2009,3,12)),
//                                                                       new Leg(ScenarioTest.Hamburg, new DateTime(2009,3,14), ScenarioTest.Stockholm, new DateTime(2009,3,15))
//                                                                    })
//                                      };
//         }
//      }
//   }
//}