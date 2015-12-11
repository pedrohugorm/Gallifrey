using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    public interface IDocumentRepository<TModel> where TModel : Microsoft.Azure.Documents.Document
    {
        IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> predicate = null);
        TModel FindById(string id);

        Task<Microsoft.Azure.Documents.Document> Insert(TModel model);
        Task<Microsoft.Azure.Documents.Document> Replace(TModel model);
        Task<Microsoft.Azure.Documents.Document> Delete(string id);
    }
}
