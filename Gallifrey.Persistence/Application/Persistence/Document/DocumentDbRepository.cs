using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Gallifrey.Persistence.Application.Persistence.Document
{
    public class DefaultDocumentDbRepository<TModel> : IDocumentRepository<TModel>
        where TModel : Microsoft.Azure.Documents.Document
    {
        private readonly DocumentClient _documentClient;
        private readonly DocumentCollection _collection;

        public DefaultDocumentDbRepository(IProvideDatabaseClient<TModel> clientProvider,
            IProvideDocumentCollection<TModel> collectionProvider)
        {
            _documentClient = clientProvider.GetClient();
            _collection = collectionProvider.ReadOrCreateCollection();
        }

        public IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> predicate = null)
        {
            var query = _documentClient.CreateDocumentQuery<TModel>(_collection.DocumentsLink);

            return (predicate == null ? query : query.Where(predicate)).AsEnumerable();
        }

        public TModel FindById(string id)
        {
            return (dynamic)GetDocumentById(id);
        }

        public async Task<Microsoft.Azure.Documents.Document> Insert(TModel model)
        {
            return await _documentClient.CreateDocumentAsync(_collection.DocumentsLink, model);
        }

        public async Task<Microsoft.Azure.Documents.Document> Replace(TModel model)
        {
            var document = GetDocumentById(model.Id);

            return await _documentClient.ReplaceDocumentAsync(document.SelfLink, model);
        }

        public async Task<Microsoft.Azure.Documents.Document> Delete(string id)
        {
            var document = GetDocumentById(id);

            return await _documentClient.DeleteDocumentAsync(document.SelfLink);
        }

        private Microsoft.Azure.Documents.Document GetDocumentById(string id)
        {
            var document =
                _documentClient.CreateDocumentQuery<Microsoft.Azure.Documents.Document>(_collection.DocumentsLink)
                    .Where(r => r.Id == id).AsEnumerable().First();
            return document;
        }
    }
}