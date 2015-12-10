namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    public interface IDocumentRepository<TModel> : IRetrieveDocumentItems<TModel>,
        IRetrieveDocumentItemById<TModel>, IPersistDocumentItem<TModel> where TModel : Microsoft.Azure.Documents.Document
    {

    }
}
