using System.Reflection;
using DDDSample.CommandHandlers;
using DDDSample.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;

using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;

namespace Tests.Integration
{
   [TestFixture]
   public abstract class ScenarioTest
   {
      public static UnLocode HONGKONG
      {
         get { return new UnLocode("CNHKG"); }
      }
      public static UnLocode STOCKHOLM
      {
         get { return new UnLocode("SESTO"); }
      }
      public static UnLocode TOKYO
      {
         get { return new UnLocode("JNTKO"); }
      }
      public static UnLocode HAMBURG
      {
         get { return new UnLocode("DEHAM"); }
      }
      public static UnLocode NEWYORK
      {
         get { return new UnLocode("USNYC"); }
      }
      public static UnLocode CHICAGO
      {
         get { return new UnLocode("USCHI"); }
      }
      public static UnLocode GOETEBORG
      {
         get { return new UnLocode("SEGOT"); }
      }

      public static ICargoRepository CargoRepository
      {
         get { return ServiceLocator.Current.GetInstance<ICargoRepository>(); }
      }

      public static ILocationRepository LocationRepository
      {
         get { return ServiceLocator.Current.GetInstance<ILocationRepository>(); }
      }

      private static IServiceLocator _ambientLocator;
      private static IUnityContainer _ambientContainer;
      protected static IDocumentStore _documentStore;

      [SetUp]
      public void Initialize()
      {
         _ambientContainer = new UnityContainer();
         _ambientLocator = new UnityServiceLocator(_ambientContainer);

         ConfigureRepositories(_ambientContainer);
         ConfigureBus(_ambientContainer);
         InitializeDocumetStore(_ambientContainer);

         ServiceLocator.SetLocatorProvider(() => _ambientLocator);
      }

      private static void ConfigureRepositories(IUnityContainer container)
      {
         container.RegisterType<ILocationRepository, LocationRepository>();
         container.RegisterType<ICargoRepository, CargoRepository>();
      } 

      private static void ConfigureBus(IUnityContainer container)
      {
         container.RegisterType<IBus, InProcessBus>();
         container.RegisterType<ICommandHandler<AssignCargoToRouteCommand>,
            AssignCargoToRouteCommandHandler>();
         container.RegisterType<ICommandHandler<BookNewCargoCommand>,
            BookNewCargoCommandHandler>();
         container.RegisterType<ICommandHandler<ChangeDestinationCommand>,
            ChangeDestinationCommandHandler>();
         container.RegisterType<ICommandHandler<RegisterHandlingEventCommand>,
            RegisterHandlingEventCommandHandler>();
      }


      private static void InitializeDocumetStore(IUnityContainer container)
      {
         var resolver = new PropertiesOnlyContractResolver
                           {
                              DefaultMembersSearchFlags =
                                 BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Instance
                           };         
         _documentStore = new DocumentStore
                             {
                                Url = "http://localhost:8080",
                                Conventions = new DocumentConvention
                                                 {
                                                    JsonContractResolver = resolver
                                                 }
                             }.Initialise();                  
         container.RegisterInstance(_documentStore);
      }

      protected static void InvokeCommand<T>(T command)
      {
         var bus = _ambientContainer.Resolve<IBus>();
         bus.Send(command);
      }

      [TearDown]
      public void TearDownTests()
      {
         _documentStore.Dispose();
      }
      
           
   }
}
