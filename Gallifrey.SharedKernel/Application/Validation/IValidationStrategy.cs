namespace Gallifrey.SharedKernel.Application.Validation
{
    /// <summary>
    /// Strategy interface to validate <typeparam name="T"></typeparam>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidationStrategy<in T> : IValidationStrategy
    {
        ValidationResult Validate(T val);
    }

    public interface IValidationStrategy
    {
        ValidationResult Validate(object val);
    }
}