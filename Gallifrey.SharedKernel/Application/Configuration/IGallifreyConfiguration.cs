using System;
using System.Collections.Generic;
using System.Data.Entity;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;

namespace Gallifrey.SharedKernel.Application.Configuration
{
    public interface IGallifreyConfiguration
    {
        /// <summary>
        /// Defines which DbContext to use
        /// </summary>
        /// <typeparam name="TDatabaseContext"></typeparam>
        void SetDatabaseContext<TDatabaseContext>() where TDatabaseContext : DbContext;

        /// <summary>
        /// Define which configurations to use
        /// </summary>
        /// <typeparam name="TPersistenceProvider"></typeparam>
        void SetPersistenceConfigurationProvider<TPersistenceProvider>()
            where TPersistenceProvider : IPersistenceConfigurationProvider;
        
        /// <summary>
        /// Defines a Default database repository to use.
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TId"></typeparam>
        void SetDatabaseRepository<TRepository, TModel, TId>()
            where TRepository : IRepository<TModel, TId>
            where TModel : class, IIdentity<TId>
            where TId : IIdentity<TId>; 

        /// <summary>
        /// Define filters for every <typeparamref name="TModel"/> when retrieving <see cref="IEnumerable{TModel}"/>
        /// </summary>
        /// <typeparam name="TFilterHandler"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        void SetFilterStrategy<TFilterHandler, TModel>()
            where TFilterHandler : IHandleModelFilterStrategy<TModel>
            where TModel : class;

        /// <summary>
        /// While adding an item, the repository triggers this strategy.
        /// That way you can define your own code to add entities
        /// </summary>
        /// <param name="strategy"></param>
        void SetAddItemStrategy(Type strategy);

        /// <summary>
        /// While updating an item, the repository triggers this strategy.
        /// That way you can define your own code to update entities
        /// </summary>
        /// <param name="strategy"></param>
        void SetUpdateItemStrategy(Type strategy);

        /// <summary>
        /// While deleting an item, the repository triggers this strategy.
        /// That way you can define your own code to update entities.
        /// You can change the way the repository removes an item, for instance:
        /// You can change it to delete logically (using a DeletedAt field, for instance)
        /// </summary>
        /// <param name="strategy"></param>
        void SetRemoveItemStrategy(Type strategy);
    }
}