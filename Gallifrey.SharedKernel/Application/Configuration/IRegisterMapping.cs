namespace Gallifrey.SharedKernel.Application.Configuration
{
    /// <summary>
    /// Interface that is called to register mappings
    /// Implement one or many to register custom mappings with automapper
    /// </summary>
    public interface IRegisterMapping
    {
        void Register();
    }
}
