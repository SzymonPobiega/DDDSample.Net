using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Application.AsynchronousEventHandlers.MessageHandlers;
using DDDSample.Messages;
using DDDSample.Reporting.Persistence.NHibernate;
using NHibernate;

namespace DDDSample.Reporting.MessageHandlers
{
   public class CargoRegisteredMessageHandler : AbstractMessageHandler<CargoRegisteredMessage>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoRegisteredMessageHandler(CargoDataAccess cargoDataAccess, ISessionFactory sessionFactory)
         : base(sessionFactory)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      protected override void DoHandle(CargoRegisteredMessage message)
      {
         Cargo cargo = new Cargo(message.CargoId, message.TrackingId, message.Origin, message.Destination, message.ArrivalDeadline);
         _cargoDataAccess.Store(cargo);
      }
   }
}