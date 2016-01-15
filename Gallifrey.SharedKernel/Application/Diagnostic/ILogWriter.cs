namespace Gallifrey.SharedKernel.Application.Diagnostic
{
    public interface ILogWriter
    {
        void Write(string message, Severity severity = Severity.Information);
        void Write(System.Exception e, Severity severity = Severity.Error);
        void Write<T>(T value, Severity severity = Severity.Information);
    }
}
