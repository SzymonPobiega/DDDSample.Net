using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Application.Commands;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using LeanCommandUnframework;

namespace DDDSample.UI.BookingAndTracking.Facade
{
    /// <summary>
    /// Facade for cargo booking services.
    /// </summary>
    public class BookingServiceFacade
    {
        private readonly PipelineFactory _pipelineFactory;
        private readonly CargoRoutingDTOAssembler _cargoRoutingAssembler;
        private readonly ILocationRepository _locationRepository;
        private readonly ICargoRepository _cargoRepository;

        public BookingServiceFacade(PipelineFactory pipelineFactory, ILocationRepository locationRepository, ICargoRepository cargoRepository, CargoRoutingDTOAssembler cargoRoutingAssembler)
        {
            _pipelineFactory = pipelineFactory;
            _cargoRoutingAssembler = cargoRoutingAssembler;
            _cargoRepository = cargoRepository;
            _locationRepository = locationRepository;
        }

        /// <summary>
        /// Books new cargo for specified origin, destination and arrival deadline.
        /// </summary>
        /// <param name="origin">Origin of a cargo in UnLocode format.</param>
        /// <param name="destination">Destination of a cargo in UnLocode format.</param>
        /// <param name="arrivalDeadline">Arrival deadline.</param>
        /// <returns>Cargo tracking id.</returns>
        public string BookNewCargo(string origin, string destination, DateTime arrivalDeadline)
        {
            var command = new BookNewCargoCommand
                              {
                                  Origin = origin,
                                  Destination = destination,
                                  ArrivalDeadline = arrivalDeadline
                              };
            var result = (BookNewCargoCommandResult) _pipelineFactory.Process(command);
            return result.TrackingId;
        }

        /// <summary>
        /// Returns a list of all defined shipping locations in format acceptable by MVC framework 
        /// drop down list.
        /// </summary>
        /// <returns>A list of shipping locations.</returns>
        public IList<SelectListItem> ListShippingLocations()
        {
            return _locationRepository.FindAll().Select(x => new SelectListItem { Text = x.Name, Value = x.UnLocode.CodeString }).ToList();
        }

        /// <summary>
        /// Loads DTO of cargo for cargo routing function.
        /// </summary>
        /// <param name="trackingId">Cargo tracking id.</param>
        /// <returns>DTO.</returns>
        public CargoRoutingDTO LoadCargoForRouting(string trackingId)
        {
            Cargo c = _cargoRepository.Find(new TrackingId(trackingId));
            if (c == null)
            {
                throw new ArgumentException("Cargo with specified tracking id not found.");
            }
            return _cargoRoutingAssembler.ToDTO(c);
        }

        /// <summary>
        /// Returns a complete list of cargos stored in the system.
        /// </summary>
        /// <returns>A collection of cargo DTOs.</returns>
        public IList<CargoRoutingDTO> ListAllCargos()
        {
            return _cargoRepository.FindAll().Select(x => _cargoRoutingAssembler.ToDTO(x)).ToList();
        }

        /// <summary>
        /// Changes destination of an existing cargo.
        /// </summary>
        /// <param name="trackingId">Cargo tracking id.</param>
        /// <param name="destination">New destination UnLocode in string format.</param>
        public void ChangeDestination(string trackingId, string destination)
        {
            var command = new ChangeDestinationCommand
                              {
                                  NewDestination = destination,
                                  TrackingId = trackingId
                              };
            _pipelineFactory.Process(command);
        }

        /// <summary>
        /// Fetches all possible routes for delivering cargo with provided tracking id.
        /// </summary>
        /// <param name="trackingId">Cargo tracking id.</param>
        /// <returns>Possible delivery routes</returns>
        public IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(String trackingId)
        {
            var command = new RequestPossibleRoutesForCargoCommand
                              {
                                  TrackingId = trackingId
                              };
            var result = (RequestPossibleRoutesForCargoCommandResult) _pipelineFactory.Process(command);
            return result.RouteCandidates;
        }

        /// <summary>
        /// Binds cargo to selected delivery route.
        /// </summary>
        /// <param name="trackingId">Cargo tracking id.</param>
        /// <param name="route">Route definition.</param>
        public void AssignCargoToRoute(String trackingId, RouteCandidateDTO route)
        {
            var command = new AssignCargoToRouteCommand
                              {
                                  TrackingId = trackingId,
                                  Route = route
                              };
            _pipelineFactory.Process(command);
        }
    }
}