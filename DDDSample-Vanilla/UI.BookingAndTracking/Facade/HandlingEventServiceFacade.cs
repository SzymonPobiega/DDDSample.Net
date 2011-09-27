using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Application.Commands;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using LeanCommandUnframework;

namespace DDDSample.UI.BookingAndTracking.Facade
{
    /// <summary>
    /// Facade for cargo handling event service.
    /// </summary>
    public class HandlingEventServiceFacade
    {
        private readonly ILocationRepository _locationRepository;
        private readonly PipelineFactory _pipelineFactory;

        public HandlingEventServiceFacade(ILocationRepository locationRepository, PipelineFactory pipelineFactory)
        {
            _locationRepository = locationRepository;
            _pipelineFactory = pipelineFactory;
        }

        public void RegisterHandlingEvent(DateTime completionTime, string trackingId, string location, HandlingEventType type)
        {
            var command = new RegisterHandlingEventCommand
                              {
                                  CompletionTime = completionTime,
                                  TrackingId = trackingId,
                                  OccuranceLocation = location,
                                  Type = type
                              };
            _pipelineFactory.Process(command);
        }

        /// <summary>
        /// Returns a list of all defined shipping locations in format acceptable by MVC framework 
        /// drop down list.
        /// </summary>
        /// <returns>A list of shipping locations.</returns>
        public IList<SelectListItem> ListHandlingLocations()
        {
            return _locationRepository.FindAll().Select(x => new SelectListItem { Text = x.Name, Value = x.UnLocode.CodeString }).ToList();
        }
    }
}