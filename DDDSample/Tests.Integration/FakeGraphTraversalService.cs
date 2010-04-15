using System;
using System.Collections.Generic;
using DDDSample.DomainModel.Potential.Location;
using DDDSample.Pathfinder;

namespace Tests.Integration
{
   //public class FakeGraphTraversalService : IGraphTraversalService
   //{
   //   public IList<TransitPath> FindPaths(string originUnLocode, string destinationUnLocode, Constraints limitations)
   //   {
   //      if (originUnLocode == SampleLocations.Hongkong.UnLocode.CodeString)
   //      {
   //         // Hongkong - NYC - Chicago - Stockholm, initial routing
   //         return new List<TransitPath>{
   //                                      new TransitPath(new List<TransitEdge>{
   //                                                                    new TransitEdge(SampleLocations.Hongkong.UnLocode.CodeString, SampleLocations.NewYork.UnLocode.CodeString, new DateTime(2009,3,03), new DateTime(2009,3,9)),
   //                                                                    new TransitEdge(SampleLocations.NewYork.UnLocode.CodeString,  SampleLocations.Chicago.UnLocode.CodeString, new DateTime(2009,3,10), new DateTime(2009,3,14)),
   //                                                                    new TransitEdge(SampleLocations.Chicago.UnLocode.CodeString,  SampleLocations.Stockholm.UnLocode.CodeString, new DateTime(2009,3,7), new DateTime(2009,3,11))
   //                                                                 })
   //                                   };
   //      }
   //      // Tokyo - Hamburg - Stockholm, rerouting misdirected cargo from Tokyo 
   //      return new List<TransitPath>{
   //                                   new TransitPath(new List<TransitEdge>{
   //                                                                 new TransitEdge(SampleLocations.Tokyo.UnLocode.CodeString,  SampleLocations.Hamburg.UnLocode.CodeString, new DateTime(2009,3,8),new DateTime(2009,3,12)),
   //                                                                 new TransitEdge(SampleLocations.Hamburg.UnLocode.CodeString,  SampleLocations.Stockholm.UnLocode.CodeString, new DateTime(2009,3,14), new DateTime(2009,3,15))
   //                                                              })
   //                                };
   //   }
   //}   
}