using System;
using System.Data.Entity;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;

namespace Gallifrey.Persistence.Application.Strategy
{
    /// <summary>
    /// Default strategy to add items in repository
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class DefaultAddItemStrategy<TModel, TId> : IAddItemStrategy<TModel, TId>
        where TModel : class, IIdentity<TId>
    {
        private void GenerateIdentity(TModel model)
        {
            if (!(model.Id is Guid)) return;

            typeof (TModel).GetProperty("Id").SetValue(model, Guid.NewGuid());
        }

        public virtual void AddItem(DbSet<TModel> dbSet, TModel model)
        {
            GenerateIdentity(model);
            dbSet.Add(model);
        }
    }
}