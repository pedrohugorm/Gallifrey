using Gallifrey.SharedKernel.Application.Persistence.Repository.Document;

namespace Gallifrey.Persistence.Application.Persistence.Document.Resolver
{
    public class DefaultCollectionIdResolver<TModel> : IResolveDocumentCollectionId<TModel>
    {
        public string Resolve()
        {
            return typeof (TModel).Name;
        }
    }
}