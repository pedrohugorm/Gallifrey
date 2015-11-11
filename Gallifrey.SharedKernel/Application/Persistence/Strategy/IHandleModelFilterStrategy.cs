using System.Linq;

namespace Gallifrey.SharedKernel.Application.Persistence.Strategy
{
    public interface IHandleModelFilterStrategy<TModel>
    {
        IQueryable<TModel> HandleFilter(IQueryable<TModel> queryable);
    }
}