using System.Collections.Generic;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Interface that has filtering operations for repository
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepositoryFilters<TModel>
    {
        IEnumerable<TModel> GetAllFiltered();

        IEnumerable<TModel> ApplyFilterAndOrdering(IEnumerable<TModel> enumerable);
    }
}