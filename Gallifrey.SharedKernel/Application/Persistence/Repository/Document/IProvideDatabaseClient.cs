using Microsoft.Azure.Documents.Client;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    // ReSharper disable once UnusedTypeParameter
    public interface IProvideDatabaseClient<TModel>
    {
        string EndpointUrl { set; get; }
        string AuthorizationKey { set; get; }

        DocumentClient GetClient();
    }
}