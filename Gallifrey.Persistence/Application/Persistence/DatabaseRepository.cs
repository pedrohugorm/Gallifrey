﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Gallifrey.Persistence.Application.Extension;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using StructureMap;
using WebGrease.Css.Extensions;

namespace Gallifrey.Persistence.Application.Persistence
{
    /// <summary>
    /// Database repository for a given <typeparam name="TModel"></typeparam> and <typeparam name="TIdentityType"></typeparam>
    /// You can extend its behavior, by defining strategies for each operation.
    /// <see cref="IHandleModelFilterStrategy{TModel}"/> to change the way the repository filters data
    /// <see cref="IAddItemStrategy{TModel,TId}"/> to change how repository inserts data
    /// <see cref="IUpdateItemStrategy{TModel,TId}"/> to change the way how repository updates data
    /// <see cref="IRemoveItemStrategy{TModel,TId}"/> to change the way how repository removes data
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TIdentityType"></typeparam>
    public class DatabaseRepository<TModel, TIdentityType> : IDatabaseRepository<TModel, TIdentityType>
        where TModel : class, IIdentity<TIdentityType>
    {
        private readonly DbContext _context;
        private readonly IPersistenceConfigurationProvider _provider;
        private readonly IContainer _container;
        private readonly IAddItemStrategy<TModel, TIdentityType> _addItemStrategy;
        private readonly IUpdateItemStrategy<TModel, TIdentityType> _updateItemStrategy;
        private readonly IRemoveItemStrategy<TModel, TIdentityType> _removeItemStrategy;

        public DatabaseRepository(DbContext context,
            IPersistenceConfigurationProvider provider,
            IContainer container, 
            IAddItemStrategy<TModel, TIdentityType> addItemStrategy,
            IUpdateItemStrategy<TModel, TIdentityType> updateItemStrategy,
            IRemoveItemStrategy<TModel, TIdentityType> removeItemStrategy)
        {
            _context = context;
            _provider = provider;
            _container = container;
            _addItemStrategy = addItemStrategy;
            _updateItemStrategy = updateItemStrategy;
            _removeItemStrategy = removeItemStrategy;

            EntityChangingHandlers = _container.GetAllInstances<IHandleEntityChanging<TModel>>();
            EntityChangedHandlers = _container.GetAllInstances<IHandleEntityChanged<TModel>>();
        }

        public virtual DbContext GetContext()
        {
            return _context;
        }

        public virtual DbSet<TModel> GetDbSet()
        {
            return GetContext().GetDbSet<TModel>();
        }

        public virtual IQueryable<TModel> GetAll()
        {
            return GetDbSet();
        }

        public virtual IQueryable<TModel> GetAllFiltered()
        {
            return ApplyFilterAndOrdering(GetAll());
        }

        /// <summary>
        /// Override to apply custom ordering/filtering
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public virtual IQueryable<TModel> ApplyFilterAndOrdering(IQueryable<TModel> enumerable)
        {
            var filters = _container.GetAllInstances<IHandleModelFilterStrategy<TModel>>().ToList();

            if(!filters.Any())
                return enumerable;

            var result = enumerable;

            foreach (var filter in filters)
            {
                result = filter.HandleFilter(enumerable);
            }

            return result;
        }

        public TModel Find(TIdentityType id)
        {
            return (_provider.IsFindFiltered ? GetAllFiltered() : GetAll()).SingleOrDefault(r => r.Id.Equals(id));
        }

        public void InsertOrUpdate(TModel model)
        {
            if (model.Id.Equals(default(TIdentityType)))
                _addItemStrategy.AddItem(GetDbSet(), model);
            else
                _updateItemStrategy.UpdateItem(GetDbSet(), model);
        }

        public void Delete(TIdentityType id)
        {
            _removeItemStrategy.RemoveItem(GetDbSet(), id);
        }

        public IEnumerable<IHandleEntityChanging<TModel>> EntityChangingHandlers { get; set; }
        public IEnumerable<IHandleEntityChanged<TModel>> EntityChangedHandlers { get; set; }

        public void DisableProxyAndLazyLoading()
        {
            GetContext().Configuration.ProxyCreationEnabled = _provider.ProxyCreationEnabled;
            GetContext().Configuration.LazyLoadingEnabled = _provider.LazyLoadingEnabled;
        }

        public void Save()
        {
            var entries = GetContext().ChangeTracker.Entries<TModel>().ToList();
            entries.ForEach(e => EntityChangingHandlers.ForEach(h => h.OnEntityChanging(e)));

            GetContext().SaveChanges();

            entries.ForEach(e => EntityChangedHandlers.ForEach(h => h.OnEntityChanged(e)));
        }
    }
}