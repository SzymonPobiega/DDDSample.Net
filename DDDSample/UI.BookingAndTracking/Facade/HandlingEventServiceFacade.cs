using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using DDDSample.Application;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Facade for cargo handling event service.
   /// </summary>
   public class HandlingEventServiceFacade
   {      
      private readonly IHandlingEventService _handlingEventService;
      private readonly ILocationRepository _locationRepository;

      public HandlingEventServiceFacade(IHandlingEventService handlingEventService, ILocationRepository locationRepository)
      {
         _handlingEventService = handlingEventService;
         _locationRepository = locationRepository;
      }

      public void RegisterHandlingEvent(DateTime completionTime, string trackingId, string location, HandlingEventType type)
      {
         _handlingEventService.RegisterHandlingEvent(
            completionTime,
            new TrackingId(trackingId),            
            new UnLocode(location),
            type);
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