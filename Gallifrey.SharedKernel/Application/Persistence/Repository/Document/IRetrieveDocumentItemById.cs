namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    public interface IRetrieveDocumentItemById<out TModel>
    {
        TModel FindById(string id);
    }
}