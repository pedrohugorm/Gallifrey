using System.Data.Entity;
using System.Linq;
using Gallifrey.Persistence.Application.Extension;
using Gallifrey.SharedKernel.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;

namespace Gallifrey.Persistence.Application.Persistence
{
    public class DatabaseRepository<TModel, TIdentityType> : IDatabaseRepository<TModel, TIdentityType>
        where TModel : class, IIdentity<TIdentityType>
    {
        private readonly DbContext _context;
        private readonly IPersistenceConfigurationProvider _provider;
        private readonly IHandleModelFilterStrategy<TModel> _modelFilterStrategy;
        private readonly IAddItemStrategy<TModel, TIdentityType> _addItemStrategy;
        private readonly IUpdateItemStrategy<TModel, TIdentityType> _updateItemStrategy;
        private readonly IRemoveItemStrategy<TModel, TIdentityType> _removeItemStrategy;

        public DatabaseRepository(DbContext context,
            IPersistenceConfigurationProvider provider,
            IHandleModelFilterStrategy<TModel> modelFilterStrategy, 
            IAddItemStrategy<TModel, TIdentityType> addItemStrategy,
            IUpdateItemStrategy<TModel, TIdentityType> updateItemStrategy,
            IRemoveItemStrategy<TModel, TIdentityType> removeItemStrategy)
        {
            _context = context;
            _provider = provider;
            _modelFilterStrategy = modelFilterStrategy;
            _addItemStrategy = addItemStrategy;
            _updateItemStrategy = updateItemStrategy;
            _removeItemStrategy = removeItemStrategy;
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
            return _modelFilterStrategy.HandleFilter(enumerable);
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

        public void DisableProxyAndLazyLoading()
        {
            GetContext().Configuration.ProxyCreationEnabled = _provider.ProxyCreationEnabled;
            GetContext().Configuration.LazyLoadingEnabled = _provider.LazyLoadingEnabled;
        }

        public void Save()
        {
            GetContext().SaveChanges();
        }
    }
}