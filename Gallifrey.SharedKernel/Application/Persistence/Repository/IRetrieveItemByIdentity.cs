namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IRetrieveItemByIdentity<out TModel, in TId> where TModel : class
    {
        TModel Find(TId id);
    }
}