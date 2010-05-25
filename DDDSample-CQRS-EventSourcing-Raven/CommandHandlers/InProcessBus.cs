using System;
using DDDSample.Domain.Persistence.NHibernate;
using Microsoft.Practices.Unity;
using Raven.Client;

namespace DDDSample.CommandHandlers
{   
   public class InProcessBus : IBus
   {
      private readonly IDocumentStore _documentStore;
      private readonly IUnityContainer _container;

      public InProcessBus(IDocumentStore documentStore, IUnityContainer container)
      {
         _documentStore = documentStore;
         _container = container;
      }

      public void Send<T>(T command)
      {
         try
         {
            UnitOfWork.Current = new UnitOfWork(_documentStore);
            ProcessCommand(command);
            UnitOfWork.Current.Commit();
         }
         catch (Exception)
         {
            UnitOfWork.Current.Rollback();
            throw;
         }
         finally
         {            
            UnitOfWork.Current = null;            
         }
      }

      private void ProcessCommand<T>(T command)
      {
         var handler = _container.Resolve<ICommandHandler<T>>();
         handler.Handle(command);
      }
   }
}