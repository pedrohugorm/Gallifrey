using System.Data.Entity.Infrastructure;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IHandleEntityChanged<TModel> where TModel : class
    {
        void OnEntityChanged(DbEntityEntry<TModel> entry);
    }
}