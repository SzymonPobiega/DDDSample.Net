using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Location;
using DDDSample.Reporting.Persistence.NHibernate;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Facade for cargo booking services.
   /// </summary>
   public class BookingServiceFacade
   {
      private readonly ILocationRepository _locationRepository;
      private readonly CargoDataAccess _cargoDataAccess;

      public BookingServiceFacade(ILocationRepository locationRepository, CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
         _locationRepository = locationRepository;
      }      

      /// <summary>
      /// Loads DTO of cargo for cargo routing function.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <returns>DTO.</returns>
      public Reporting.Cargo LoadCargoForRouting(string trackingId)
      {
         Reporting.Cargo c = _cargoDataAccess.Find(trackingId);
         if (c == null)
         {
            throw new ArgumentException("Cargo with specified tracking id not found.");
         }
         return c;
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
      /// Returns a complete list of cargos stored in the system.
      /// </summary>
      /// <returns>A collection of cargo DTOs.</returns>
      public IList<Reporting.Cargo> ListAllCargos()
      {
         return _cargoDataAccess.FindAll();
      }               
   }
}