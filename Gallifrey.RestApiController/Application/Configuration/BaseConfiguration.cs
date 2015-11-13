using System.Data.Entity;
using Gallifrey.Persistence.Application.Persistence;
using Gallifrey.Persistence.Application.Strategy;
using Gallifrey.SharedKernel.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using Gallifrey.SharedKernel.Application.Validation;
using StructureMap;
using StructureMap.Graph;

namespace Gallifrey.RestApi.Application.Configuration
{
    public abstract class BaseConfiguration : IGallifreyConfiguration
    {
        private readonly IContainer _container;

        protected BaseConfiguration(IContainer container)
        {
            _container = container;
            _container.Configure(x =>
            {
                x.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.RegisterConcreteTypesAgainstTheFirstInterface();
                    s.AddAllTypesOf(typeof(IValidationStrategy<>));
                    s.AddAllTypesOf(typeof(IPersistItem<,>));
                    s.AddAllTypesOf(typeof(IRetrieveItemByIdentity<,>));
                    s.AddAllTypesOf(typeof(IRetrieveQueryOfItems<>));
                    s.AddAllTypesOf(typeof(IRepository<,>));
                    s.AddAllTypesOf(typeof(IDatabaseRepository<,>));
                    s.AddAllTypesOf(typeof(IIdentity<>));
                });

                x.For(typeof(IRepository<,>)).Use(typeof(DatabaseRepository<,>));

                x.For<IPersistenceConfigurationProvider>().Use<DefaultPersistenceConfiguration>();

                x.For(typeof(IHandleModelFilterStrategy<>)).Use(typeof(NullHandleModelFilterStrategy<>));
                x.For(typeof(IAddItemStrategy<,>)).Use(typeof(DefaultAddItemStrategy<,>));
                x.For(typeof(IUpdateItemStrategy<,>)).Use(typeof(DefaultUpdateItemStrategy<,>));
                x.For(typeof(IRemoveItemStrategy<,>)).Use(typeof(DefaultRemoveItemStrategy<,>));
            });
        }

        public void SetDatabaseContext<TDatabaseContext>() where TDatabaseContext : DbContext
        {
            _container.Configure(x => x.For(typeof (DbContext)).Use(typeof (TDatabaseContext)));
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

        public void SetAddItemStrategy<TAddItemStrategy, TModel, TId>(TAddItemStrategy strategy)
            where TAddItemStrategy : IAddItemStrategy<TModel, TId>
            where TModel : class, IIdentity<TId>
        {
            _container.Configure(x => x.For(typeof (IAddItemStrategy<,>)).Use(typeof (TAddItemStrategy)));
        }

        public void SetUpdateItemStrategy<TUpdateItemStrategy, TModel, TId>(TUpdateItemStrategy strategy)
            where TUpdateItemStrategy : IUpdateItemStrategy<TModel, TId>
            where TModel : class, IIdentity<TId>
        {
            _container.Configure(x => x.For(typeof(IUpdateItemStrategy<,>)).Use(typeof(TUpdateItemStrategy)));
        }

        public void SetRemoveItemStrategy<TRemoveItemStrategy, TModel, TId>(TRemoveItemStrategy strategy)
            where TRemoveItemStrategy : IRemoveItemStrategy<TModel, TId>
            where TModel : class, IIdentity<TId>
        {
            _container.Configure(x => x.For(typeof (IRemoveItemStrategy<,>)).Use(typeof (TRemoveItemStrategy)));
        }
    }
}
