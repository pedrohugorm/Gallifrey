using System;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;
using Microsoft.Azure.Documents.Client;

namespace Gallifrey.Persistence.Application.Persistence.Document.Provider
{
    public class DefaultDocumentDatabaseClientProvider<TModel> : IProvideDatabaseClient<TModel>
    {
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }

        public DefaultDocumentDatabaseClientProvider(string endpointUrl, string authorizationKey)
        {
            EndpointUrl = endpointUrl;
            AuthorizationKey = authorizationKey;
        }

        public DocumentClient GetClient()
        {
            return new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
        }
    }
}