namespace Gallifrey.SharedKernel.Application.Serialization
{
    public interface ISerializeObject
    {
        string Serialize<T>(T value);
    }
}
