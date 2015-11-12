namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Interface that has poersistance operations for a given <typeparam name="TModel"></typeparam>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IPersistItem<in TModel, in TId> where TModel : class
    {
        void InsertOrUpdate(TModel model);
        void Delete(TId id);
    }
}