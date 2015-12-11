using System;
using System.Diagnostics;
using System.Linq;
using FunctionalTest.Domain;
using Gallifrey.RestApi.Application.Configuration;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StructureMap;

namespace FunctionalTest.Repository
{
    [TestClass]
    public class DocumentDbRepositoryTest
    {
        [TestInitialize]
        public void Init()
        {
            
        }

        [TestCleanup]
        public void Finish()
        {
            var repository = GetRepository();

            var items = repository.GetAll();
            foreach (var item in items)
            {
                repository.Delete(item.Id);
                Debug.WriteLine("DELETED " + item.Id);
            }
        }

        IContainer GetContainer()
        {
            var container = new Container();
            var config = new DefaultConfiguration(container);
            config.UsingCustomRegistry(
                new DefaultDocumentDbRegistry()
                    .DatabaseClientFor<SpecialDeal>(
                        "https://store.documents.azure.com:443/",
                        "xJZ/t6iycMQUwQcwlUNS/xTer2EfEoN6MDln8ud8oYwjsPf4o8uLXsLnNifk96SvSeHglQPIiworTHuiiWXuww==")
                    .DatabaseIdFor<SpecialDeal>("mealbooking")
                );

            return container;
        }

        IDocumentRepository<SpecialDeal> GetRepository()
        {
            return GetContainer().GetInstance<IDocumentRepository<SpecialDeal>>();
        }

        [TestMethod]
        public void ShouldGetGenericRepository()
        {
            var resolver = new RepositoryTypeProvider();
            var repoType = resolver.GetRepository<TestModel>();

            Assert.IsTrue(typeof (IDatabaseRepository<TestModel, Guid>) == repoType);
        }
            
        [TestMethod]
        public void ShouldInsertNewRecord()
        {
            var repository = GetRepository();

            var item = new SpecialDeal
            {
                Title = "Meal for dates",
                Description = "Get your meal ready",
                ShortText = "Short description of the item"
            };

            var result = repository.Insert(item).Result;
            Debug.WriteLine(result.Id);
        }

        Document Insert(Guid guid)
        {
            var repository = GetRepository();

            var id = guid.ToString();

            Debug.WriteLine("INSERT " + id);

            var item = new SpecialDeal
            {
                Id = id,
                Title = "Meal for dates",
                Description = "Get your meal ready",
                ShortText = "Short description of the item"
            };

            return repository.Insert(item).Result;
        }

        Document Replace(SpecialDeal item)
        {
            var repository = GetRepository();

            Debug.WriteLine("REPLACE " + item.Id);

            return repository.Replace(item).Result;
        }

        SpecialDeal FindById(Guid id)
        {
            var repository = GetRepository();

            Debug.WriteLine("FIND " + id);

            return repository.FindById(id.ToString());
        }

        Document Delete(Guid id)
        {
            var repository = GetRepository();

            Debug.WriteLine("DELETE " + id);

            return repository.Delete(id.ToString()).Result;
        }

        [TestMethod]
        public void ShouldRetrieveOneRecord()
        {
            var id = Guid.NewGuid();
            Insert(id);

            var retrieved = FindById(id);

            Assert.IsNotNull(retrieved);
        }

        [TestMethod]
        public void ShouldInsertAndDeleteRecord()
        {
            var id = Guid.NewGuid();
            var doc = Insert(id);

            Assert.AreEqual(id.ToString(), doc.Id);

            var retrieved = FindById(id);

            Debug.WriteLine(retrieved.ToString());
        }

        [TestMethod]
        public void ShouldInsertAndUpdateRecord()
        {
            var id = Guid.NewGuid();
            SpecialDeal item = (dynamic)Insert(id);

            var ticks = string.Format("{0}", DateTime.Now.Ticks);
            item.Description = ticks;

            Replace(item);
            var updated = FindById(id);

            Assert.IsTrue(updated.Description.Contains(ticks));
        }

        [TestMethod]
        public void ShouldRetrieveRecords()
        {
            Insert(Guid.NewGuid());
            Insert(Guid.NewGuid());
            Insert(Guid.NewGuid());

            var repository = GetRepository();

            var items = repository.GetAll();

            Assert.IsTrue(items.Any());
        }
    }

    internal class SpecialDeal : Document
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "shortText")]
        public string ShortText { get; set; }
    }
}
