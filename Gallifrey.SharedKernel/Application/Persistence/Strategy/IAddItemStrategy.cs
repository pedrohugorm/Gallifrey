using System.Data.Entity;

namespace Gallifrey.SharedKernel.Application.Persistence.Strategy
{
    public interface IAddItemStrategy<TModel, in TId> where TModel : class
    {
        void AddItem(DbSet<TModel> dbSet, TModel model);
    }
}