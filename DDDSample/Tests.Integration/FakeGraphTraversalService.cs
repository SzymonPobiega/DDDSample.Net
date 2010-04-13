using System;
using System.Collections.Generic;
using DDDSample.Pathfinder;

namespace Tests.Integration
{
   public class FakeGraphTraversalService : IGraphTraversalService
   {
      public IList<TransitPath> FindPaths(string originUnLocode, string destinationUnLocode, Constraints limitations)
      {
         if (originUnLocode == ScenarioTest.HONGKONG.UnLocode.CodeString)
         {
            // Hongkong - NYC - Chicago - Stockholm, initial routing
            return new List<TransitPath>{
                                         new TransitPath(new List<TransitEdge>{
                                                                       new TransitEdge(ScenarioTest.HONGKONG.UnLocode.CodeString, ScenarioTest.NEWYORK.UnLocode.CodeString, new DateTime(2009,3,03), new DateTime(2009,3,9)),
                                                                       new TransitEdge(ScenarioTest.NEWYORK.UnLocode.CodeString,  ScenarioTest.CHICAGO.UnLocode.CodeString, new DateTime(2009,3,10), new DateTime(2009,3,14)),
                                                                       new TransitEdge(ScenarioTest.CHICAGO.UnLocode.CodeString,  ScenarioTest.STOCKHOLM.UnLocode.CodeString, new DateTime(2009,3,7), new DateTime(2009,3,11))
                                                                    })
                                      };
         }
         // Tokyo - Hamburg - Stockholm, rerouting misdirected cargo from Tokyo 
         return new List<TransitPath>{
                                      new TransitPath(new List<TransitEdge>{
                                                                    new TransitEdge(ScenarioTest.TOKYO.UnLocode.CodeString,  ScenarioTest.HAMBURG.UnLocode.CodeString, new DateTime(2009,3,8),new DateTime(2009,3,12)),
                                                                    new TransitEdge(ScenarioTest.HAMBURG.UnLocode.CodeString,  ScenarioTest.STOCKHOLM.UnLocode.CodeString, new DateTime(2009,3,14), new DateTime(2009,3,15))
                                                                 })
                                   };
      }
   }   
}