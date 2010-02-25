using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Application;
using DDDSample.Reporting.Persistence.NHibernate;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Facade for cargo handling event service.
   /// </summary>
   public class HandlingEventServiceFacade
   {      
      private readonly IHandlingEventService _handlingEventService;
      private readonly ILocationRepository _locationRepository;
      private readonly CargoDataAccess _cargoDataAccess;

      public HandlingEventServiceFacade(IHandlingEventService handlingEventService, ILocationRepository locationRepository, CargoDataAccess cargoDataAccess)
      {
         _handlingEventService = handlingEventService;
         _cargoDataAccess = cargoDataAccess;
         _locationRepository = locationRepository;
      }

      public void RegisterHandlingEvent(DateTime completionTime, string trackingId, string location, HandlingEventType type)
      {
         Guid cargoId = _cargoDataAccess.Find(trackingId).Id;
         _handlingEventService.RegisterHandlingEvent(
            cargoId,
            completionTime,                        
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