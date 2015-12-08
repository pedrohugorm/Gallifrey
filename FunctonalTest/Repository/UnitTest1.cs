using System;
using System.Linq;
using Gallifrey.Persistence.Application.Persistence.NoSql;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionalTest.Repository
{
    [TestClass]
    public class DocumentDbRepositoryTest   
    {
        void Test()
        {
            var endpointUrl = "https://store.documents.azure.com:443/";
            var authorizationKey = "xJZ/t6iycMQUwQcwlUNS/xTer2EfEoN6MDln8ud8oYwjsPf4o8uLXsLnNifk96SvSeHglQPIiworTHuiiWXuww==";
            var databaseId = "mealbooking";

            var client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

            var database = client.CreateDatabaseQuery().AsEnumerable().FirstOrDefault(db => db.Id == databaseId);

            if (database == null)
                throw new Exception(string.Format("database '{0}' not found", databaseId));

            var docCollectionName = typeof(SpecialDeal).Name;
            var docCollection =
                client.CreateDocumentCollectionQuery(string.Format("dbs/{0}", database.Id))
                    .AsEnumerable()
                    .FirstOrDefault(c => c.Id == docCollectionName) ??
                client.CreateDocumentCollectionAsync(string.Format("dbs/{0}", database.Id),
                    new DocumentCollection
                    {
                        Id = docCollectionName
                    }).Result;

            var deal = new SpecialDeal
            {
                Id = Guid.NewGuid(),
                Title = "Meal for dates",
                Description = "Get your meal ready",
                Guid = Guid.NewGuid(),
                ShortText = "Short description of the item"
            };

            var result = client.CreateDocumentAsync(string.Format("dbs/{0}/colls/{1}", database.Id, docCollection.Id), deal).Result;
        }

        IRepository<SpecialDeal, Guid> GetRepository()
        {
            var config = new BaseDocumentDbRepositoryConfiguration<SpecialDeal>
            {
                EndpointUrl = "https://store.documents.azure.com:443/",
                AuthorizationKey =
                    "xJZ/t6iycMQUwQcwlUNS/xTer2EfEoN6MDln8ud8oYwjsPf4o8uLXsLnNifk96SvSeHglQPIiworTHuiiWXuww==",
                DatabaseId = "mealbooking"
            };

            var repository = new DocumentDbRepository<SpecialDeal>(config,
                new DocumentClient(new Uri(config.EndpointUrl), config.AuthorizationKey));

            return repository;
        }
            
        [TestMethod]
        public void ShouldInsertNewRecord()
        {
            var repository = GetRepository();

            var item = new SpecialDeal
            {
                Title = "Meal for dates",
                Description = "Get your meal ready",
                Guid = Guid.NewGuid(),
                ShortText = "Short description of the item"
            };

            repository.InsertOrUpdate(item);
            repository.Save();
        }

        [TestMethod]
        public void ShouldRetrieveRecords()
        {
            var repository = GetRepository();
            
            var items = repository.GetAll();

            Assert.IsTrue(items.Any());
        }

        [TestMethod]
        public void ShouldRetrieveOneRecord()
        {

        }

        [TestMethod]
        public void ShouldInsertAndDeleteRecord()
        {

        }

        [TestMethod]
        public void ShouldInsertAndUpdateRecord()
        {

        }
    }

    internal class SpecialDeal : BaseDocumentDbDocument
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid Guid { get; set; }
        public string ShortText { get; set; }
    }
}
