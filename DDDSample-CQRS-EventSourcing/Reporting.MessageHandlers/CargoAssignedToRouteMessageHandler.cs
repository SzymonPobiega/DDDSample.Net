using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Application.AsynchronousEventHandlers.MessageHandlers;
using DDDSample.Messages;
using DDDSample.Reporting.Persistence.NHibernate;
using NHibernate;

namespace DDDSample.Reporting.MessageHandlers
{
   /// <summary>
   /// Handles <see cref="CargoAssignedToRouteMessage"/> and ensures that proper handling history
   /// obejct has been created.
   /// </summary>
   public class CargoAssignedToRouteMessageHandler : AbstractMessageHandler<CargoAssignedToRouteMessage>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoAssignedToRouteMessageHandler(CargoDataAccess cargoDataAccess, ISessionFactory sessionFactory)
         : base(sessionFactory)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      protected override void DoHandle(CargoAssignedToRouteMessage message)
      {
         Cargo cargo = _cargoDataAccess.Find(message.TrackingId);
         cargo.RouteSpecification = message.Legs;         
      }
   }
}