using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;

namespace Gallifrey.Persistence.Application.Strategy
{
    /// <summary>
    /// Null strategy that filters nothing - create a new one extending this to alter its behavior
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