using Gallifrey.Persistence.Application.Persistence;
using Gallifrey.SharedKernel.Application.Persistence.Repository;
using StructureMap.Configuration.DSL;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class DefaultDatabaseRegistry : Registry
    {
        public DefaultDatabaseRegistry()
        {
            For(typeof(IRepository<,>)).Use(typeof(DatabaseRepository<,>));
            For(typeof(IDatabaseRepository<,>)).Use(typeof(DatabaseRepository<,>));
        }
    }
}