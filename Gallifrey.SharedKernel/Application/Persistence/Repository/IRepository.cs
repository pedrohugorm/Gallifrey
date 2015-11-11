namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IRepository
    {

    }

    public interface IRepository<TModel, in TId> : IRepository, IRetrieveItemByIdentity<TModel, TId>,
        IRetrieveQueryOfItems<TModel>, IPersistItem<TModel, TId> where TModel : class
    {
        void Save();
    }
}