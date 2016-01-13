using System.Data.Entity;
using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using Omu.ValueInjecter;

namespace Gallifrey.Persistence.Application.Strategy
{
    /// <summary>
    /// Default strategy to update items in repository
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class DefaultUpdateItemStrategy<TModel, TId> : IUpdateItemStrategy<TModel, TId>
        where TModel : class, IIdentity<TId>
    {
        public virtual void UpdateItem(DbSet<TModel> dbSet, TModel model)
        {
            var originalModel = dbSet.Find(model.Id);

            //TODO a way to extend this
            originalModel.InjectFrom(model);
        }
    }
}