using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Application.AsynchronousEventHandlers.MessageHandlers;
using DDDSample.Messages;
using DDDSample.Reporting.Persistence.NHibernate;
using NHibernate;

namespace DDDSample.Reporting.MessageHandlers
{
   public class CargoDestinationChangedMessageHandler : AbstractMessageHandler<CargoDestinationChangedMessage>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoDestinationChangedMessageHandler(CargoDataAccess cargoDataAccess, ISessionFactory sessionFactory)
         : base(sessionFactory)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      protected override void DoHandle(CargoDestinationChangedMessage message)
      {
         Cargo cargo = _cargoDataAccess.Find(message.CargoId);
         cargo.UpdateRouteSpecification(message.Origin, message.Destination, message.ArrivalDeadline);
      }
   }
}