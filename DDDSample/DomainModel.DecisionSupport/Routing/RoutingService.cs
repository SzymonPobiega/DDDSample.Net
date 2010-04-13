using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Policy.Commitments;
using DDDSample.DomainModel.Potential.Location;
using DDDSample.Pathfinder;

namespace DDDSample.DomainModel.DecisionSupport.Routing
{   
   public class RoutingService : IRoutingService
   {
      private readonly ILocationRepository _locatinRepository;
      private readonly IGraphTraversalService _graphTraversalService;
      private readonly ICustomerAgreementRepository _agreementRepository;

      public RoutingService(ILocationRepository locatinRepository, IGraphTraversalService graphTraversalService, ICustomerAgreementRepository agreementRepository)
      {
         _locatinRepository = locatinRepository;
         _graphTraversalService = graphTraversalService;
         _agreementRepository = agreementRepository;
      }

      public IList<Itinerary> FetchRoutesFor(Cargo cargoToBeRouted)
      {
         var possibleRoutes = GetAllPossibleRoutes(cargoToBeRouted.RouteSpecification);         
         var agreement = _agreementRepository.Find(cargoToBeRouted.OrderingCustomer);

         return agreement.RoutingPolicy.SelectOptimal(possibleRoutes).ToList();
      }

      private IEnumerable<Itinerary> GetAllPossibleRoutes(RouteSpecification routeSpecification)
      {
         var allPaths = _graphTraversalService.FindPaths(
            routeSpecification.Origin.UnLocode.CodeString,
            routeSpecification.Destination.UnLocode.CodeString,
            new Constraints(routeSpecification.ArrivalDeadline));
         return allPaths.Select(x => ToItinerary(x));
      }

      private Itinerary ToItinerary(TransitPath path)
      {
         return new Itinerary(path.Edges.Select(x => ToLeg(x)));
      }

      private Leg ToLeg(TransitEdge edge)
      {
         return new Leg(
            _locatinRepository.Find(new UnLocode(edge.From)), 
            edge.FromDate,
            _locatinRepository.Find(new UnLocode(edge.To)),
            edge.ToDate);
      }
   }
}