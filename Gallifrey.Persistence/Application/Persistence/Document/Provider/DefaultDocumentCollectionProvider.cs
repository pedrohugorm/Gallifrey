using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Gallifrey.Persistence.Application.Persistence.Document.Provider
{
    public class DefaultDocumentCollectionProvider<TModel> : IProvideDocumentCollection<TModel>
    {
        private readonly IProvideDatabaseClient<TModel> _clientProvider;
        private readonly IProvideDocumentDatabase<TModel> _databaseProvider;
        private readonly string _collectionId;

        public DefaultDocumentCollectionProvider(IProvideDatabaseClient<TModel> clientProvider,
            IProvideDocumentDatabase<TModel> databaseProvider, IResolveDocumentCollectionId<TModel> collectionIdResolver)
        {
            _clientProvider = clientProvider;
            _databaseProvider = databaseProvider;
            _collectionId = collectionIdResolver.Resolve();
        }

        public DocumentCollection ReadOrCreateCollection()
        {
            var client = _clientProvider.GetClient();

            var col = client.CreateDocumentCollectionQuery(_databaseProvider.ReadOrCreateDatabase().SelfLink)
                .Where(c => c.Id == _collectionId)
                .AsEnumerable()
                .FirstOrDefault();

            if (col != null) return col;

            var collectionSpec = new DocumentCollection {Id = _collectionId};
            var requestOptions = new RequestOptions {OfferType = "S1"};

            col = client.CreateDocumentCollectionAsync(_databaseProvider.ReadOrCreateDatabase().SelfLink,
                collectionSpec, requestOptions).Result;

            return col;
        }
    }
}