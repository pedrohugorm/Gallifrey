using Newtonsoft.Json;

namespace Gallifrey.SharedKernel.Application.Extension
{
    public class SimpleStacktraceDetail
    {
        [JsonProperty(PropertyName = "c")]
        public string ClassName { set; get; }
        [JsonProperty(PropertyName = "f")]
        public string File { set; get; }
        [JsonProperty(PropertyName = "me")]
        public string Method { set; get; }
        [JsonProperty(PropertyName = "l")]
        public int LineNumber { set; get; }
    }
}