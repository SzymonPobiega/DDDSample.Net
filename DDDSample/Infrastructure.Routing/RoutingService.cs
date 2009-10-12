using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;

namespace Infrastructure.Routing
{
   public class RoutingService : IRoutingService
   {
      private readonly ILocationRepository _locatinRepository;

      public RoutingService(ILocationRepository locatinRepository)
      {
         _locatinRepository = locatinRepository;
      }

      public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
      {
         Location through =
            _locatinRepository.FindAll().Except(new[] {routeSpecification.Destination, routeSpecification.Origin}).First();
         DateTime throughArrival = DateTime.Now + TimeSpan.FromSeconds((routeSpecification.ArrivalDeadline - DateTime.Now).TotalSeconds/2);

         List<Itinerary> results = new List<Itinerary>
                                      {
                                         new Itinerary(new[]
                                                          {
                                                             new Leg(routeSpecification.Origin, DateTime.Now,
                                                                     through,
                                                                     throughArrival),
                                                             new Leg(through, throughArrival,
                                                                     routeSpecification.Destination,
                                                                     routeSpecification.ArrivalDeadline)
                                                          })
                                      };
         return results;
      }
   }
}
