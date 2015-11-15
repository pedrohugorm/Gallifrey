namespace Gallifrey.SharedKernel.Application.Validation
{
    public interface IValidationHelper<in TModel>
    {
        void Validate(TModel model);
    }

    public interface IValidate<in TModel>
    {
        FluentValidation.Results.ValidationResult Validate(TModel model);
    }
}