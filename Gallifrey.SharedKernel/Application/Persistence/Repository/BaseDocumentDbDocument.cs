using System;
using Newtonsoft.Json;

namespace Gallifrey.SharedKernel.Application.Persistence.Repository
{
    public abstract class BaseDocumentDbDocument : IIdentity<Guid>
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
    }
}