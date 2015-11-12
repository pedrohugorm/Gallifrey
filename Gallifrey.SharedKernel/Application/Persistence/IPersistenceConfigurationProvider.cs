namespace Gallifrey.SharedKernel.Application.Persistence
{
    /// <summary>
    /// Persistence configuration class
    /// </summary>
    public interface IPersistenceConfigurationProvider
    {
        bool ProxyCreationEnabled { set; get; }
        bool LazyLoadingEnabled { set; get; }
        bool IsFindFiltered { set; get; }
    }
}
