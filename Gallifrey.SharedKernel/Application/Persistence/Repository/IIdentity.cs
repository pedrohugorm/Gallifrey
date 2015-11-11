namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    /// <summary>
    /// Every model of your database must implement this interface.
    /// In order to do that, you need to change your .tt template generator - if you are using .edmx file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentity<T>
    {
        T Id { get; set; }
    }
}