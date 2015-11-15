using StructureMap;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class DefaultConfiguration : BaseConfiguration
    {
        public DefaultConfiguration(IContainer container) : base(container)
        {
            UsingDefaultDatabaseRepository();
            UsingDefaultCrudStrategies();
        }

        public DefaultConfiguration() : this(new Container())
        {
            
        }
    }
}