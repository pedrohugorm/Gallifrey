using Microsoft.Azure.Documents;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    // ReSharper disable once UnusedTypeParameter
    public interface IProvideDocumentDatabase<TModel>
    {
        Database ReadOrCreateDatabase();
    }
}