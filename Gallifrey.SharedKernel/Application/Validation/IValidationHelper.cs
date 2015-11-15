namespace Gallifrey.SharedKernel.Application.Validation
{
    public interface IValidationHelper<in TModel>
    {
        void Validate(TModel model);
    }
}