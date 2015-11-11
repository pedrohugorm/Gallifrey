using System.Data.Entity;

namespace Gallifrey.SharedKernel.Application.Persistence.Strategy
{
    public interface IRemoveItemStrategy<TModel, in TId> where TModel : class
    {
        void RemoveItem(DbSet<TModel> dbSet, TId id);
    }
}