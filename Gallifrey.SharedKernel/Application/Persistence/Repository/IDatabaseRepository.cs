﻿using System.Collections.Generic;
using System.Data.Entity;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IDatabaseRepository<TModel, in TIdentityType> : IRepository<TModel, TIdentityType>,
        IRepositoryFilters<TModel>
        where TModel : class
    {
        IEnumerable<IHandleEntityChanging<TModel>> EntityChangingHandlers { set; get; } 
        IEnumerable<IHandleEntityChanged<TModel>> EntityChangedHandlers { set; get; } 

        void DisableProxyAndLazyLoading();

        DbContext GetContext();

        DbSet<TModel> GetDbSet();
    }
}