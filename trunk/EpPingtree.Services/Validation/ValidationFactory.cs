using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Services.Validation.Interfaces;
using FluentValidation;

namespace EpPingtree.Services.Validation
{
    public class ValidationFactory<TModel> : AbstractValidator<TModel>, IValidationFactory
    {

    }
}
