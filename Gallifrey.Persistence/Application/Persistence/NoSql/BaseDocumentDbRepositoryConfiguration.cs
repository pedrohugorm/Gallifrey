namespace Gallifrey.Persistence.Application.Persistence.NoSql
{
    public class BaseDocumentDbRepositoryConfiguration<TModel> : IProvideDocumentDbRepositoryConfiguration<TModel>
    {
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionName { get; set; }
        public bool IsShouldCreateDocumentCollections { get; set; }

        public BaseDocumentDbRepositoryConfiguration()
        {
            CollectionName = typeof(TModel).Name;
            IsShouldCreateDocumentCollections = true;
        }
    }
}