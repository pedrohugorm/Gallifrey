using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IHandleEntityChanging<TModel> where TModel : class
    {
        void OnEntityChanging(DbEntityEntry<TModel> entry);
    }
}