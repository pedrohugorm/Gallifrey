namespace Gallifrey.SharedKernel.Application.Persistence
{
    public class DefaultPersistenceConfiguration : IPersistenceConfigurationProvider
    {
        public bool ProxyCreationEnabled { get; set; }
        public bool LazyLoadingEnabled { get; set; }
        public bool IsFindFiltered { get; set; }

        public DefaultPersistenceConfiguration()
        {
            ProxyCreationEnabled = false;
            LazyLoadingEnabled = false;
            IsFindFiltered = true;
        }
    }
}
