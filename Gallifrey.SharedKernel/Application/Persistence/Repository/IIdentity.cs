namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public interface IIdentity<T>
    {
        T Id { get; set; }
    }
}