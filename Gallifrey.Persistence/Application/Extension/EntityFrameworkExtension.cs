using System.Data.Entity;
using System.Linq;
using System.Management.Instrumentation;

namespace Gallifrey.Persistence.Application.Extension
{
    /// <summary>
    /// Entity extension to get a <see cref="DbSet{TEntity}"/> when you don't know which properties you have.
    /// </summary>
    public static class EntityFrameworkExtension
    {
        public static DbSet<T> GetDbSet<T>(this DbContext context) where T : class
        {
            var type = context.GetType();
            var properties = type.GetProperties();

            if (properties.All(r => r.PropertyType != typeof (DbSet<T>)))
                throw new InstanceNotFoundException(
                    string.Format("Could not find {0} instance using EntityFrameworkHelper.GetDbSet<T>", typeof (T).Name));

            var property = properties.First(r => r.PropertyType == typeof(DbSet<T>));
            return property.GetValue(context) as DbSet<T>;
        }
    }
}