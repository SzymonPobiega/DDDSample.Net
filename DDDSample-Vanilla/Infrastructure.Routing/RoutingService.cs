using System.Collections.Generic;
using System.Linq;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Voyage;
using DDDSample.Pathfinder;

namespace Infrastructure.Routing
{   
   public class RoutingService : IRoutingService
   {
      private readonly ILocationRepository _locatinRepository;
      private readonly IGraphTraversalService _graphTraversalService;
      private readonly IVoyageRepository _voyageRepository;

      public RoutingService(ILocationRepository locatinRepository, IGraphTraversalService graphTraversalService, IVoyageRepository voyageRepository)
      {
         _locatinRepository = locatinRepository;
         _voyageRepository = voyageRepository;
         _graphTraversalService = graphTraversalService;
      }

      public IList<Itinerary> FetchRoutesForSpecification(RouteSpecification routeSpecification)
      {
         return GetAllPossibleRoutes(routeSpecification).ToList();         
      }

      private IEnumerable<Itinerary> GetAllPossibleRoutes(RouteSpecification routeSpecification)
      {
         IList<Voyage> voyages = _voyageRepository.FindBeginingBefore(routeSpecification.ArrivalDeadline);
         IList<TransitEdge> graph = voyages.SelectMany(v => v.Schedule.CarrierMovements.Select(m => ToEdge(v, m))).ToList();

         var allPaths = _graphTraversalService.FindPaths(
            routeSpecification.Origin.UnLocode.CodeString,
            routeSpecification.Destination.UnLocode.CodeString,
            graph,
            new Constraints(routeSpecification.ArrivalDeadline));
         return allPaths.Select(x => ToItinerary(x));
      }

      private Itinerary ToItinerary(TransitPath path)
      {
         return new Itinerary(CreateLegs(path.Edges));
      }

      private IEnumerable<Leg> CreateLegs(IEnumerable<TransitEdge> pathEdges)
      {         
         var enumerator = pathEdges.GetEnumerator();
         enumerator.MoveNext();
         while (true)
         {
            TransitEdge first = enumerator.Current;
            TransitEdge last = enumerator.Current;
            bool hasMore;
            while ((hasMore = enumerator.MoveNext()) && enumerator.Current.Key == first.Key)
            {
               last = enumerator.Current;
            }
            yield return ToLeg((Voyage)first.Key, first, last);
            if (!hasMore)
            {
               break;
            }            
         }
      }

      private Leg ToLeg(Voyage voyage, TransitEdge first, TransitEdge last)
      {
         return new Leg(voyage, _locatinRepository.Find(new UnLocode(first.From)), 
            first.FromDate,
            _locatinRepository.Find(new UnLocode(last.To)),
            last.ToDate);
      }

      private static TransitEdge ToEdge(Voyage voyage, CarrierMovement movement)
      {
         return new TransitEdge(voyage,
            movement.TransportLeg.DepartureLocation.UnLocode.CodeString,
            movement.TransportLeg.ArrivalLocation.UnLocode.CodeString,
            movement.DepartureTime,
            movement.ArrivalTime);
      }
   }
}