using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Reporting.Persistence.NHibernate;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Handling event registration service.
   /// </summary>
   public class HandlingEventService : IHandlingEventService
   {
      private readonly CargoDataAccess _cargoDataAccess;
      private readonly ICargoRepository _cargoRepository;

      public HandlingEventService(ICargoRepository cargoRepository, CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
         _cargoRepository = cargoRepository;
      }

      public void RegisterHandlingEvent(Guid cargoId, DateTime completionTime, UnLocode unLocode, HandlingEventType type)
      {
         Cargo cargo = _cargoRepository.Find(cargoId);   
               
         cargo.RegisterHandlingEvent(type, unLocode, DateTime.Now, completionTime);
      }
   }
}