using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Gallifrey.SharedKernel.Application.Extension
{
    public class SimpleExceptionDetail
    {
        [JsonProperty(PropertyName = "m")]
        public string Message { set; get; }
        [JsonProperty(PropertyName = "t")]
        public string Type { set; get; }
        [JsonProperty(PropertyName = "s")]
        public IEnumerable<SimpleStacktraceDetail> StacktraceDetails { set; get; }

        public SimpleExceptionDetail()
        {
            StacktraceDetails = new List<SimpleStacktraceDetail>();
        }

        public SimpleExceptionDetail(System.Exception e, bool isIncludeStack = true)
            : this()
        {
            Message = e.Message;
            Type = e.GetType().FullName;

            if (!isIncludeStack) return;
            var frames = new StackTrace(e, true).GetFrames();

            if (frames == null || !frames.Any()) return;

            StacktraceDetails = frames.Select(r => new SimpleStacktraceDetail
            {
                File = r.GetFileName(),
                LineNumber = r.GetFileLineNumber(),
                Method = r.GetMethod().Name
            });
        }
    }
}