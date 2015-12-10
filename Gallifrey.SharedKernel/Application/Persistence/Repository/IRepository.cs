using System;
using System.Collections.Generic;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IRepository
    {

    }

    /// <summary>
    /// Repository interface that does Read/Write operations using a <typeparam name="TModel"></typeparam> and a <typeparam name="TId"></typeparam>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<TModel, in TId> : IRepository, IRetrieveItemByIdentity<TModel, TId>,
        IRetrieveQueryOfItems<TModel>, IPersistItem<TModel, TId> where TModel : class
    {
        void Save();
    }
}