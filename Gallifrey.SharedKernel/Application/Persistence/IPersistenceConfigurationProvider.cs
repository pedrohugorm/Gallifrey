namespace Gallifrey.SharedKernel.Application.Persistence
{
    public interface IPersistenceConfigurationProvider
    {
        bool ProxyCreationEnabled { set; get; }
        bool LazyLoadingEnabled { set; get; }
        bool IsFindFiltered { set; get; }
    }
}
