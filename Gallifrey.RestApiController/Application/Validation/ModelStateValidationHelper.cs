using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using FluentValidation;
using Gallifrey.SharedKernel.Application.Validation;
using Microsoft.Ajax.Utilities;

namespace Gallifrey.RestApi.Application.Validation
{
    public class ModelStateValidationHelper<TModel> : IValidationHelper<TModel>
    {
        private readonly ModelStateDictionary _modelState;
        private readonly IEnumerable<IValidator<TModel>> _validators;

        public ModelStateValidationHelper(ModelStateDictionary modelState, IEnumerable<IValidator<TModel>> validators)
        {
            _modelState = modelState;
            _validators = validators;
        }

        public void Validate(TModel model)
        {
            var validationResults = _validators.Select(r => r.Validate(model)).ToList();

            if (validationResults.All(r => r.IsValid))
                return;

            var validationErrors = validationResults.SelectMany(r => r.Errors);
            validationErrors.ForEach(r => _modelState.AddModelError(r.PropertyName ?? "Validation", r.ErrorMessage));
        }
    }
}