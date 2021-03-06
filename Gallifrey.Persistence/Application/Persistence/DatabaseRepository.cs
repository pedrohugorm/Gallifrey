﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Gallifrey.Persistence.Application.Extension;
using Gallifrey.SharedKernel.Application.Diagnostic;
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
        private readonly ILogWriter _logWriter;

        public DatabaseRepository(DbContext context,
            IPersistenceConfigurationProvider provider,
            IContainer container, 
            IAddItemStrategy<TModel, TIdentityType> addItemStrategy,
            IUpdateItemStrategy<TModel, TIdentityType> updateItemStrategy,
            IRemoveItemStrategy<TModel, TIdentityType> removeItemStrategy,
            ILogWriter logWriter)
        {
            _context = context;
            _provider = provider;
            _container = container;
            _addItemStrategy = addItemStrategy;
            _updateItemStrategy = updateItemStrategy;
            _removeItemStrategy = removeItemStrategy;
            _logWriter = logWriter;
        }

        public virtual DbContext GetContext()
        {
            return _context;
        }

        public virtual DbSet<TModel> GetDbSet()
        {
            return GetContext().GetDbSet<TModel>();
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            return GetDbSet();
        }

        public virtual IEnumerable<TModel> GetAllFiltered()
        {
            return ApplyFilterAndOrdering(GetAll());
        }

        /// <summary>
        /// Override to apply custom ordering/filtering
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public virtual IEnumerable<TModel> ApplyFilterAndOrdering(IEnumerable<TModel> enumerable)
        {
            var filters = _container.GetAllInstances<IHandleModelFilterStrategy<TModel>>().ToList();

            if(!filters.Any())
                return enumerable;

            // ReSharper disable once PossibleMultipleEnumeration TODO - Confirm that
            var result = enumerable;

            foreach (var filter in filters)
            {
                result = filter.HandleFilter(enumerable);
            }

            return result;
        }

        public TModel Find(TIdentityType id)
        {
            var query = (_provider.IsFindFiltered ? GetAllFiltered() : GetAll());

            if (id is Guid)
            {
                var list = new[] {id};

                return query.SingleOrDefault(r => list.Contains(r.Id));
            }
            
            return query.SingleOrDefault(r => r.Id.Equals(id));
        }

        public void InsertOrUpdate(TModel model)
        {
            _logWriter.Write(
                string.Format("Insert/Update, Value = {0} | Check => IsInt = {1}, {2} | IsGuid = {3}, {4}",
                    model.Id,
                    model.Id is int,
                    model.Id as int?,
                    model.Id is Guid,
                    model.Id as Guid?
                    ));

            if ((model.Id is int && (model.Id as int? == 0)) 
                || (model.Id is Guid && (model.Id as Guid? == Guid.Empty)))
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

        private readonly IList<EntityChangedEvent<TModel>> _postSaveEventsToTrigger = new List<EntityChangedEvent<TModel>>();

        public void Save()
        {
            //TODO get this from configuration
            var triggerEventsForStateList = new []
            {
                EntityState.Added,
                EntityState.Deleted,
                EntityState.Modified
            };

            var entries =
                GetContext()
                    .ChangeTracker.Entries<TModel>()
                    .Where(r => triggerEventsForStateList.Contains(r.State))
                    .ToList();

            TriggerEventsBeforePersisting(entries);

            GetContext().SaveChanges();

            TriggerEventsAfterPersisting();
        }

        void TriggerEventsBeforePersisting(IEnumerable<DbEntityEntry<TModel>> entries)
        {
            var entityChangingHandlers = _container.GetAllInstances<IHandleEntityChanging<TModel>>();

            //Trigger events for entities before persisting
            entries.ForEach(e =>
                entityChangingHandlers.ForEach(h =>
                {
                    h.OnEntityChanging(e);
                    _postSaveEventsToTrigger.Add(new EntityChangedEvent<TModel>(e.Entity, e.State));
                }));
        }

        void TriggerEventsAfterPersisting()
        {
            var entityChangedHandlers = _container.GetAllInstances<IHandleEntityChanged<TModel>>();

            //Trigger events for entities after persisting
            _postSaveEventsToTrigger.ForEach(e => entityChangedHandlers.ForEach(h => h.OnEntityChanged(e)));
        }
    }
}