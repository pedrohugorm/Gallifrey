using System.Collections.Generic;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Retrieves a queriable of <typeparam name="TModel"></typeparam> with no filters
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRetrieveQueryOfItems<out TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll();
    }
}