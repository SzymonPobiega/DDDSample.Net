using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Potential.Location;

namespace DDDSample.Application.Services
{
   /// <summary>
   /// Handling event service.
   /// </summary>
   public interface IHandlingEventService
   {      
      /// <summary>
      /// Registers a handling event in the system, and notifies interested parties that a 
      /// cargo has been handled.
      /// </summary>
      /// <param name="completionTime">Date and time when the event was completed.</param>
      /// <param name="trackingId">Cargo tracking id.</param>      
      /// <param name="unLocode">UN locode for the location where the event occurred.</param>
      /// <param name="type">Type of event.</param> 
      /// <exception cref="CannotCreateHandlingEventException">if a handling event that represents 
      /// an actual event that's relevant to a cargo we're tracking can't be created from the 
      /// parameters </exception>
      void RegisterHandlingEvent(DateTime completionTime, TrackingId trackingId, UnLocode unLocode, HandlingEventType type);
   }
}