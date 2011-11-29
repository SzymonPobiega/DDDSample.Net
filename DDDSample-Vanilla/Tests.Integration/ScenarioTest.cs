using System;
using System.Collections.Generic;
using Autofac;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Domain.Persistence.NHibernate;
using DDDSample.UI.BookingAndTracking.Composition;
using LeanCommandUnframework;
using NHibernate;
using NHibernate.ByteCode.LinFu;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;

namespace Tests.Integration
{
    [TestFixture]
    public abstract class ScenarioTest
    {
        public static Location Hongkong
        {
            get { return LocationRepository.Find(new UnLocode("CNHKG")); }
        }
        public static Location Stockholm
        {
            get { return LocationRepository.Find(new UnLocode("SESTO")); }
        }
        public static Location Tokyo
        {
            get { return LocationRepository.Find(new UnLocode("JNTKO")); }
        }
        public static Location Hamburg
        {
            get { return LocationRepository.Find(new UnLocode("DEHAM")); }
        }
        public static Location NewYork
        {
            get { return LocationRepository.Find(new UnLocode("USNYC")); }
        }
        public static Location Chicago
        {
            get { return LocationRepository.Find(new UnLocode("USCHI")); }
        }

        public static ICargoRepository CargoRepository
        {
            get { return _ambientContainer.Resolve<ICargoRepository>(); }
        }

        public static ILocationRepository LocationRepository
        {
            get { return _ambientContainer.Resolve<ILocationRepository>(); }
        }

        public static PipelineFactory CommandPipeline
        {
            get { return _ambientContainer.Resolve<PipelineFactory>(); }
        }

        private static IContainer _ambientContainer;
        private static ISessionFactory _sessionFactory;

        private ISession _currentSession;

        [SetUp]
        public void Initialize()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<ApplicationServicesModule>();
            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<EventPublisherModule>();
            containerBuilder.RegisterModule<AutofacObjectFactoryModule>();
            containerBuilder.RegisterModule<ControllerModule>();
            containerBuilder.RegisterModule<FacadeModule>();
            containerBuilder.RegisterModule<DTOAssemblerModule>();
            containerBuilder.RegisterModule<ExternalServicesModule>();

            containerBuilder.RegisterType<TransactionCommandFilter>().AsSelf();
            containerBuilder.RegisterInstance(new FilterSelector(typeof(TransactionCommandFilter)));
            InitializeNHibernate(containerBuilder);

            containerBuilder.Register(x => _ambientContainer);

            _ambientContainer = containerBuilder.Build();
        }

        [TearDown]
        public void TearDownTests()
        {
            _sessionFactory.Dispose();
        }

        private void InitializeNHibernate(ContainerBuilder builder)
        {
            Configuration cfg = new Configuration()
               .AddProperties(new Dictionary<string, string>
                              {
                                 {Environment.ConnectionDriver, typeof (SQLite20Driver).FullName},
                                 {Environment.Dialect, typeof (SQLiteDialect).FullName},
                                 {Environment.ConnectionProvider, typeof (DriverConnectionProvider).FullName},
                                 {Environment.ConnectionString, "Data Source=:memory:;Version=3;New=True;"},
                                 {
                                    Environment.ProxyFactoryFactoryClass,
                                    typeof (ProxyFactoryFactory).AssemblyQualifiedName
                                    },
                                 {
                                    Environment.CurrentSessionContextClass,
                                    typeof (ThreadStaticSessionContext).AssemblyQualifiedName
                                    },
                                 {Environment.ReleaseConnections,"on_close"},
                                 {Environment.Hbm2ddlAuto, "create"},
                                 {Environment.ShowSql, true.ToString()}
                              });
            cfg.AddAssembly("DDDSample.Domain.Persistence.NHibernate");

            _sessionFactory = cfg.BuildSessionFactory();
            builder.RegisterInstance(_sessionFactory);

            ISession session = _sessionFactory.OpenSession();

            new SchemaExport(cfg).Execute(false, true, false, session.Connection, Console.Out);

            SampleLocations.CreateLocations(session);
            SampleTransportLegs.CreateTransportLegs(session);
            SampleVoyages.CreateVoyages(session);
            session.Flush();

            _currentSession = session;

            CurrentSessionContext.Bind(_currentSession);
        }
    }
}
