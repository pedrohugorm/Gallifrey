using System.Linq;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Interface that has filtering operations for repository
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepositoryFilters<TModel>
    {
        IQueryable<TModel> GetAllFiltered();

        IQueryable<TModel> ApplyFilterAndOrdering(IQueryable<TModel> enumerable);
    }
}