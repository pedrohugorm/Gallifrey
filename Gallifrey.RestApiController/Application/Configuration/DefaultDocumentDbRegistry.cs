using Gallifrey.Persistence.Application.Persistence.Document;
using Gallifrey.Persistence.Application.Persistence.Document.Provider;
using Gallifrey.Persistence.Application.Persistence.Document.Resolver;
using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;
using StructureMap.Configuration.DSL;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class DefaultDocumentDbRegistry : Registry
    {
        public DefaultDocumentDbRegistry()
        {
            Scan(s =>
            {
                s.AddAllTypesOf(typeof(IDocumentRepository<>));
            });

            For(typeof(IResolveDocumentCollectionId<>)).Use(typeof(DefaultCollectionIdResolver<>));
            For(typeof(IProvideDocumentCollection<>)).Use(typeof(DefaultDocumentCollectionProvider<>));
            For(typeof(IDocumentRepository<>)).Use(typeof(DefaultDocumentDbRepository<>));
        }

        public DefaultDocumentDbRegistry DatabaseIdForAll(string databaseId)
        {
            For(typeof(IProvideDocumentDatabase<>)).Use(typeof(DefaultDocumentDatabaseProvider<>))
                .Ctor<string>("databaseId").Is(databaseId);

            return this;
        }

        public DefaultDocumentDbRegistry DatabaseIdFor<TModel>(string databaseId)
        {
            For<IProvideDocumentDatabase<TModel>>().Use<DefaultDocumentDatabaseProvider<TModel>>()
                .Ctor<string>("databaseId").Is(databaseId);

            return this;
        }

        public DefaultDocumentDbRegistry DatabaseClientForAll(string endpointUrl, string authorizationKey)
        {
            For(typeof (IProvideDatabaseClient<>)).Use(typeof (DefaultDocumentDatabaseClientProvider<>))
                .Ctor<string>("endpointUrl")
                .Is(endpointUrl)
                .Ctor<string>("authorizationKey")
                .Is(authorizationKey);

            return this;
        }

        public DefaultDocumentDbRegistry DatabaseClientFor<TModel>(string endpointUrl, string authorizationKey)
        {
            For<IProvideDatabaseClient<TModel>>()
                .Use<DefaultDocumentDatabaseClientProvider<TModel>>()
                .Ctor<string>("endpointUrl")
                .Is(endpointUrl)
                .Ctor<string>("authorizationKey")
                .Is(authorizationKey);

            return this;
        }
    }
}