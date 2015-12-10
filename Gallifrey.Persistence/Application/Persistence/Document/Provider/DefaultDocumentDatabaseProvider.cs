using System.Linq;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Gallifrey.Persistence.Application.Persistence.Document.Provider
{
    public class DefaultDocumentDatabaseProvider<TModel> : IProvideDocumentDatabase<TModel>
    {
        private readonly DocumentClient _client;
        private readonly string _databaseId;

        public DefaultDocumentDatabaseProvider(IProvideDatabaseClient<TModel> clientProvider, string databaseId)
        {
            _client = clientProvider.GetClient();
            _databaseId = databaseId;
        }

        public Database ReadOrCreateDatabase()
        {
            var db = _client.CreateDatabaseQuery()
                .Where(d => d.Id == _databaseId)
                .AsEnumerable()
                .FirstOrDefault() ?? _client.CreateDatabaseAsync(new Database {Id = _databaseId}).Result;

            return db;
        }
    }
}