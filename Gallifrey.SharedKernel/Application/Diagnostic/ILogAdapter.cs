namespace Gallifrey.SharedKernel.Application.Diagnostic
{
    public interface ILogAdapter
    {
        void Write(string message, Severity severity = Severity.Information);
    }
}