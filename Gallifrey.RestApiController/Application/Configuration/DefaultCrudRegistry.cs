using Gallifrey.Persistence.Application.Strategy;
using Gallifrey.SharedKernel.Application.Persistence.Strategy;
using StructureMap.Configuration.DSL;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class DefaultCrudRegistry : Registry
    {
        public DefaultCrudRegistry()
        {
            For(typeof(IAddItemStrategy<,>)).Add(typeof(DefaultAddItemStrategy<,>));
            For(typeof(IUpdateItemStrategy<,>)).Add(typeof(DefaultUpdateItemStrategy<,>));
            For(typeof(IRemoveItemStrategy<,>)).Add(typeof(DefaultRemoveItemStrategy<,>));
        }
    }
}