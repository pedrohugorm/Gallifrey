using System.Threading.Tasks;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    public interface IPersistDocumentItem<in TModel>
    {
        Task<Microsoft.Azure.Documents.Document> Insert(TModel model);
        Task<Microsoft.Azure.Documents.Document> Replace(TModel model);
        Task<Microsoft.Azure.Documents.Document> Delete(string id);
    }
}