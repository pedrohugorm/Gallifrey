namespace Gallifrey.SharedKernel.Application.Serialization
{
    public class JsonSerializer : ISerializeObject, IDeserializeObject
    {
        public string Serialize<T>(T value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
    }
}