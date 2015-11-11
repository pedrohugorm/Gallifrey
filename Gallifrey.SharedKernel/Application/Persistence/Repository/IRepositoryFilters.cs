using System.Linq;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IRepositoryFilters<TModel>
    {
        IQueryable<TModel> GetAllFiltered();

        IQueryable<TModel> ApplyFilterAndOrdering(IQueryable<TModel> enumerable);
    }
}