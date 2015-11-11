using System.Linq;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Retrieves a queriable of <typeparam name="TModel"></typeparam> with no filters
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRetrieveQueryOfItems<out TModel> where TModel : class
    {
        IQueryable<TModel> GetAll();
    }
}