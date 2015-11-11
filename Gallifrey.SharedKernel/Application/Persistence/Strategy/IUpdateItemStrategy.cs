using System.Data.Entity;

namespace Gallifrey.SharedKernel.Application.Persistence.Strategy
{
    public interface IUpdateItemStrategy<TModel, in TId> where TModel : class
    {
        void UpdateItem(DbSet<TModel> dbSet, TModel model);
    }
}