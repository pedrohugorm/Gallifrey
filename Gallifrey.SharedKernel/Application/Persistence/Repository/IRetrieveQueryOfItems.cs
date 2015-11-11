using System.Linq;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IRetrieveQueryOfItems<out TModel> where TModel : class
    {
        IQueryable<TModel> GetAll();
    }
}