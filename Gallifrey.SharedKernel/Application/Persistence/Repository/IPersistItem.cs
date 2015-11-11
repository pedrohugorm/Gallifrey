using System;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IPersistItem<in TModel, in TId> where TModel : class
    {
        void InsertOrUpdate(TModel model);
        void Delete(TId id);
    }
}