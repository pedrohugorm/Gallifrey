using System.Data.Entity;
using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using Omu.ValueInjecter;

namespace Gallifrey.Persistence.Application.Strategy
{
    public class DefaultUpdateItemStrategy<TModel, TId> : IUpdateItemStrategy<TModel, TId>
        where TModel : class, IIdentity<TId>
    {
        public virtual void UpdateItem(DbSet<TModel> dbSet, TModel model)
        {
            var originalModel = dbSet.SingleOrDefault(r => r.Id.Equals(model.Id));

            originalModel.InjectFrom(model);
        }
    }
}