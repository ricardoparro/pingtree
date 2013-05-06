using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Results;

namespace EpPingtree.Services.Validation
{
    public class BaseValidator<TModel> : AbstractValidator<TModel>
    {
        private List<IValidator> _validatorsBefore = null;

        protected void SetValidatorBefore<TBaseModel>(IValidator<TBaseModel> validator)
        {
            if (_validatorsBefore == null)
                _validatorsBefore = new List<IValidator>();

            _validatorsBefore.Add(validator);
        }

        public override ValidationResult Validate(TModel instance)
        {
            //Merge the Validation Results from the other validators
            ValidationResult result = new ValidationResult();

            if (_validatorsBefore != null)
            {
                foreach (IValidator validationRules in _validatorsBefore)
                {
                    ValidationResult validationResult = validationRules.Validate(instance);
                    MergeValidationResult(validationResult, result);
                }
            }

            ValidationResult thisResult = base.Validate(instance);
            MergeValidationResult(thisResult, result);

            return result;
        }

        private void MergeValidationResult(ValidationResult copyFrom, ValidationResult copyTo)
        {
            foreach (ValidationFailure failure in copyFrom.Errors)
            {
                if (!copyTo.Errors.Any(a => a.PropertyName == failure.PropertyName))
                    //If error doesn't already exist for this property, add it
                    copyTo.Errors.Add(failure);
            }
        }
    }
}
