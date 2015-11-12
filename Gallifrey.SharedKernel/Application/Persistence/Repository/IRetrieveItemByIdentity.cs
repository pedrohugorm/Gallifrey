namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Retrieves a <typeparam name="TModel"></typeparam> by finding it using <typeparam name="TId"></typeparam>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRetrieveItemByIdentity<out TModel, in TId> where TModel : class
    {
        TModel Find(TId id);
    }
}