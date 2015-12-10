using System.Collections.Generic;

namespace Gallifrey.SharedKernel.Application.Persistence.Strategy
{
    public interface IHandleModelFilterStrategy<TModel>
    {
        IEnumerable<TModel> HandleFilter(IEnumerable<TModel> queryable);
    }
}