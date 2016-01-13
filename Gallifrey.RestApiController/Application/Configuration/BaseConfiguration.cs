using System;
using System.Data.Entity;
using System.Reflection;
using FluentValidation;
using Gallifrey.SharedKernel.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using StructureMap;
using StructureMap.Configuration.DSL;
using WebGrease.Css.Extensions;

namespace Gallifrey.RestApi.Application.Configuration
{
    public abstract class BaseConfiguration : IGallifreyConfiguration
    {
        private readonly IContainer _container;

        protected BaseConfiguration() : this(new Container())
        {
        }

        protected BaseConfiguration(IContainer container)
        {
            _container = container;
            _container.Configure(x => x.AddRegistry<BaseRegistry>());

            //Register all mappings found
            _container.GetAllInstances<IRegisterMapping>().ForEach(r => r.Register());
        }

        /// <summary>
        /// Use default database repository configuration
        /// </summary>
        public BaseConfiguration UsingDefaultDatabaseRepository()
        {
            _container.Configure(x => x.AddRegistry<DefaultDatabaseRegistry>());

            return this;
        }

        public BaseConfiguration UsingDefaultCrudStrategies()
        {
            _container.Configure(x => x.AddRegistry<DefaultCrudRegistry>());

            return this;
        }

        public BaseConfiguration UsingCustomRegistry(Registry registry)
        {
            _container.Configure(x => x.AddRegistry(registry));

            return this;
        }

        public BaseConfiguration RegisterValidationsInAssembly(Assembly assembly)
        {
            AssemblyScanner.FindValidatorsInAssembly(assembly)
                .ForEach(result =>
                {
                    _container.Configure(
                        x => x.For(result.InterfaceType).Singleton().Use(result.ValidatorType));
                });

            return this;
        }

        public void SetDatabaseContext<TDatabaseContext>() where TDatabaseContext : DbContext
        {
            _container.Configure(x => x.For<DbContext>().Use<TDatabaseContext>());
        }

        public void SetPersistenceConfigurationProvider<TPersistenceProvider>()
            where TPersistenceProvider : IPersistenceConfigurationProvider
        {
            _container.Configure(x => x.For<IPersistenceConfigurationProvider>().Use<TPersistenceProvider>());
        }

        public void SetDatabaseRepository<TRepository, TModel, TId>()
            where TRepository : IRepository<TModel, TId>
            where TModel : class, IIdentity<TId>
            where TId : IIdentity<TId>
        {
            _container.Configure(x => x.For(typeof (IRepository<,>)).Use(typeof (TRepository)));
        }

        public void SetFilterStrategy<TFilterHandler, TModel>()
            where TFilterHandler : IHandleModelFilterStrategy<TModel>
            where TModel : class
        {
            _container.Configure(x => x.For(typeof (IHandleModelFilterStrategy<>)).Use(typeof (TFilterHandler)));
        }

        public void SetAddItemStrategy(Type strategy)
        {
            _container.Configure(x => x.For(typeof(IAddItemStrategy<,>)).Use(strategy));
        }

        public void SetUpdateItemStrategy(Type strategy)
        {
            _container.Configure(x => x.For(typeof(IUpdateItemStrategy<,>)).Use(strategy));
        }

        public void SetRemoveItemStrategy(Type strategy)
        {
            _container.Configure(x => x.For(typeof(IRemoveItemStrategy<,>)).Use(strategy));
        }
    }
}
