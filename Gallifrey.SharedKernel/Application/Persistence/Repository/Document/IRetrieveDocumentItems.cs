using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository.Document
{
    public interface IRetrieveDocumentItems<TModel>
    {
        IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> predicate = null);
    }
}