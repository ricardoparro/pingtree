using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using FluentValidation.Validators;

namespace EpPingtree.Services.Validation
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> OnlyDigits<T>(this IRuleBuilder<T, string> ruleBuilder, char[] escapeCharacters)
        {
            return ruleBuilder.SetValidator(new OnlyDigitsValidator(escapeCharacters));
        }

        public static IRuleBuilderOptions<T, string> OnlyDigits<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return OnlyDigits(ruleBuilder, new char[] { });
        }

        public static IRuleBuilderOptions<T, int?> Range<T>(this IRuleBuilder<T, int?> ruleBuilder, int min, int max)
        {
            return ruleBuilder.SetValidator(new InclusiveBetweenValidator(min, max));
        }

        #region Validators

        private class OnlyDigitsValidator : PropertyValidator
        {
            private readonly char[] _escapeCharacters;

            public OnlyDigitsValidator(char[] escapeCharacters)
                : base("This field is not only digits")
            {
                _escapeCharacters = escapeCharacters;
            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                string toValidate = context.PropertyValue as string;

                if (!string.IsNullOrEmpty(toValidate))
                {
                    foreach (char c in toValidate)
                    {

                        if (!char.IsDigit(c) && !_escapeCharacters.Contains(c))
                            return false;
                    }
                }

                return true;
            }
        }

        #endregion
    }
}
