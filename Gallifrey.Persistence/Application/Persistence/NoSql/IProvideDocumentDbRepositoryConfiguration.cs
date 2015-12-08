namespace Gallifrey.Persistence.Application.Persistence.NoSql
{
    public interface IProvideDocumentDbRepositoryConfiguration<TModel>
    {
        string EndpointUrl { set; get; }
        string AuthorizationKey { set; get; }
        string DatabaseId { set; get; }
        string CollectionName { set; get; }

        bool IsShouldCreateDocumentCollections { set; get; }
    }
}