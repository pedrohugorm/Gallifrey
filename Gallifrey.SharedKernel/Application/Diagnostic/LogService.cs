using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gallifrey.SharedKernel.Application.Extension;
using Gallifrey.SharedKernel.Application.Serialization;

namespace Gallifrey.SharedKernel.Application.Diagnostic
{
    public class LogService : ILogWriter
    {
        private readonly ISerializeObject _serialize;
        private readonly IEnumerable<ILogAdapter> _logAdapters;

        public LogService(ISerializeObject serialize, IEnumerable<ILogAdapter> logAdapters)
        {
            _serialize = serialize;
            _logAdapters = logAdapters;
        }

        public void Write(string message, Severity severity = Severity.Information)
        {
            if (_logAdapters == null || !_logAdapters.Any())
            {
                Debug.Write("No log adapters found");
                return;
            }

            foreach (var adapter in _logAdapters)
            {
                adapter.Write(message);
            }
        }

        public void Write(System.Exception e, Severity severity = Severity.Error)
        {
            Write(_serialize.Serialize(e.FlattenHierarchy()));
        }

        public void Write<T>(T value, Severity severity = Severity.Information)
        {
            Write(_serialize.Serialize(value));
        }
    }
}