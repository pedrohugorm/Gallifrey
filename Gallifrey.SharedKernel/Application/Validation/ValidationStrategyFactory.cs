using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace Gallifrey.SharedKernel.Application.Validation
{
    public class ValidationStrategyFactory<T>
    {
        private readonly Action<string> _addErrorMessageAction;
        private readonly IEnumerable<IValidationStrategy> _strategies;

        public bool IsSuccess { get; private set; }

        public ValidationStrategyFactory(Action<string> addErrorMessageAction,
            IEnumerable<IValidationStrategy> strategies)
        {
            _addErrorMessageAction = addErrorMessageAction;
            _strategies = strategies;
        }

        public void Validate(T value)
        {
            if (_strategies == null) return;

            if (value == null)
            {
                IsSuccess = false;
                _addErrorMessageAction("Value is null");
                return;
            }

            var validationResults = _strategies.Select(r => r.Validate(value)).ToList();
            IsSuccess = validationResults.All(r => r.IsSuccess);
            validationResults.ForEach(r => r.Messages.ForEach(x => _addErrorMessageAction(x)));
        }
    }
}