using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;

namespace Gallifrey.Persistence.Application.Strategy
{
    /// <summary>
    /// No filter defined Strategy
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class NullHandleModelFilterStrategy<TModel> : IHandleModelFilterStrategy<TModel>
    {
        public virtual IQueryable<TModel> HandleFilter(IQueryable<TModel> queryable)
        {
            return queryable;
        }
    }
}