using System.Data.Entity;
using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;

namespace Gallifrey.Persistence.Application.Strategy
{
    public class DefaultRemoveItemStrategy<TModel, TId> : IRemoveItemStrategy<TModel, TId>
        where TModel : class, IIdentity<TId>
    {
        public virtual void RemoveItem(DbSet<TModel> dbSet, TId id)
        {
            dbSet.Remove(dbSet.SingleOrDefault(r => r.Id.Equals(id)));
        }
    }
}