﻿using System.Collections.Generic;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IRepository
    {

    }

    public interface IRepository<TModel> : IRepository
    {
        IEnumerable<TModel> GetAll();
        void InsertOrUpdate(TModel model);
    }

    /// <summary>
    /// Repository interface that does Read/Write operations using a <typeparam name="TModel"></typeparam> and a <typeparam name="TId"></typeparam>
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IRepository<TModel, in TId> : IRepository<TModel> where TModel : class
    {
        TModel Find(TId id);
        void Delete(TId id);
        void Save();
    }
}