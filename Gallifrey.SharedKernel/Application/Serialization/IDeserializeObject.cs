namespace Gallifrey.SharedKernel.Application.Serialization
{
    public interface IDeserializeObject
    {
        T Deserialize<T>(string value);
    }
}