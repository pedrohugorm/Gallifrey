using StructureMap;

namespace Gallifrey.RestApi.Application.Configuration
{
    public class DefaultConfiguration : BaseConfiguration
    {
        public DefaultConfiguration() : base(new Container())
        {
        }
    }
}