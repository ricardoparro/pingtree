using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Request;
using EpPingtree.Model.Enums;
using FluentValidation;
using FluentValidation.Results;

namespace EpPingtree.Services.Validation.LeadLoanRequest
{
    public class BaseLeadValidator: BaseValidator<LeadRequest>
    {

        private const string POSTCODE_REGEX = "^(GIR 0AA|[A-PR-UWYZ]([0-9]{1,2}|([A-HK-Y][0-9]|[A-HK-Y][0-9]([0-9]|[ABEHMNPRV-Y]))|[0-9][A-HJKS-UW])([ ]{0,1})[0-9][ABD-HJLNP-UW-Z]{2})$";
        private const string EMAIL_REGEX = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public BaseLeadValidator()
        {
            //Don't keep validating field is one rule fails
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(a => a.LoanAmount)
                .NotEmpty().WithMessage("Loan Amount is required");

            RuleFor(a => a.PaybackDate)
                .GreaterThan(DateTime.Now).WithMessage("Please provide your next pay date");
               
                // .Must(a => !_schedulingService.IsWeekendOrPublicHoliday(a)).WithMessage("Your next pay date can't be on a weekend or a holiday");

            RuleFor(a => a.EmailAddress)
              .NotEmpty().WithMessage("Email Address is required")
              .Matches(EMAIL_REGEX).WithMessage("{0} is an invalid email address", a => a.EmailAddress)
              .Length(1, 150).WithMessage("Email is too long");

            RuleFor(a => a.MobilePhone)
                .NotNull().WithMessage("Mobile Number is required")
                  .NotEmpty().WithMessage("Mobile Number is Required")
                  .Must(a => a.StartsWith("07")).WithMessage("Mobile number {0} needs to start with 07", a => a.MobilePhone)
                  .Length(11).WithMessage("Mobile number {0} is invalid", a => a.MobilePhone);

            //validate income freq

            string[] frequencies = ValueEnums.GetIncomeEnumList<ValueEnums.IncomeFrequency>();

            RuleFor(a => a.IncomeFrequency).Must(frequencies.Contains).WithMessage("Income Frequency is invalid");

            //validate home status
            string[] homeStatuses = ValueEnums.GetIncomeEnumList<ValueEnums.ResidentialStatus>();
            RuleFor(a => a.HomeStatus).Must(homeStatuses.Contains).WithMessage("Home Status is invalid");

            //validate card type
            string[] cardTypes = ValueEnums.GetIncomeEnumList<ValueEnums.CardType>();
            RuleFor(a => a.DebitCardType).Must(cardTypes.Contains).WithMessage("Card Type is invalid");

            //validate employment type
            string[] employmentTypes = ValueEnums.GetIncomeEnumList<ValueEnums.EmploymentStatus>();
            RuleFor(a => a.EmploymentStatus).Must(employmentTypes.Contains).WithMessage("Employment Status is invalid");

            //Name fields
            RuleFor(a => a.Title)
                .Must(a => string.IsNullOrEmpty(a) || a.Length <= 20).WithMessage("Title {0} is too long", a => a.Title);

            RuleFor(a => a.Forename)
                .NotEmpty().WithMessage("First Name is required")
                .Length(1, 50).WithMessage("First name {0} is too long", a => a.Forename);

            RuleFor(a => a.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Must(a => a.Length <= 50).WithMessage("Surname {0} is too long", a => a.Surname).Must(a => a.Length >= 2).WithMessage("Surname {0} is too short", a => a.Surname);

            RuleFor(a => a.Dob).NotNull().WithMessage("Dob can't be null").NotEmpty().WithMessage("Dob is required");

            //Employment fields
            RuleFor(a => a.EmployersName)
                .NotEmpty().WithMessage("Employers Name is Required")
                .Length(3, 50).WithMessage("Employers Name {0} is too long", a => a.EmployersName);

            RuleFor(a => a.IncomeFrequency)
                .NotEmpty().WithMessage("Income Frequency is required")
                .Length(1, 50).WithMessage("Income Frequency {0} is too long", a => a.IncomeFrequency);

            RuleFor(a => a.EmploymentStatus)
                .NotEmpty().WithMessage("Employment Status is required")
                .Length(1, 20).WithMessage("EmploymentStatus {0} is too long", a => a.EmploymentStatus);

            //Check Months at employer
            RuleFor(a => a.MonthsAtEmployer)
                .NotEmpty().WithMessage("Months At Employer is required")
                .GreaterThan(0).WithMessage("Months At Employer must be greater than 0");

            RuleFor(a => a.MonthlyIncome)
                .NotEmpty().WithMessage("Monthly Income is required")
                .GreaterThan(0).WithMessage("MonthlyIncome {0} has to be > 0", a => a.MonthlyIncome)
                .LessThan(10000).WithMessage("Monthly Income {0} is invalid", a => a.MonthlyIncome);

            RuleFor(a => a.AddressLine1)
                .NotEmpty().WithMessage("Address Line 1 is required");

         //   RuleFor(a => a.County)
           //     .NotEmpty().WithMessage("County is required");

            RuleFor(a => a.Town)
                .NotEmpty().WithMessage("Town is required");

            RuleFor(a => a.Postcode)
                .NotEmpty().WithMessage("Postcode is required")
                .Matches(POSTCODE_REGEX).WithMessage("{0} is an invalid postcode", a => a.Postcode)
                .Must(a => char.IsDigit(a[a.Length - 3])).WithMessage("{0} is an invalid postcode", a => a.Postcode)    //3rd last char must be number
                .Length(1, 50).WithMessage("Post code {0} too long", a => a.Postcode);


            //Check got work phone
            RuleFor(a => a.WorkPhone)
                .NotEmpty().WithMessage("Work Phone is Required")
                .Length(10, 11).WithMessage("Work Phone {0} must be 11 digits", a => a.WorkPhone);

            //Check Got Home Phone
            RuleFor(a => a.HomePhone)
                .NotEmpty().WithMessage("Home Phone is Required")
                .Length(10, 11).WithMessage("Home Phone {0} must be 11 digits", a => a.HomePhone);


            //Check Months at address
            RuleFor(a => a.MonthsAtAddress)
                .NotEmpty().WithMessage("Months At Address is required")
                .GreaterThan(0).WithMessage("Months At address must be greater than 0");

            RuleFor(a => a.BankSortcode)
                .NotEmpty().WithMessage("Sort code required")
                .OnlyDigits().WithMessage("Invalid Sort Code {0}", a => a.BankSortcode)
                .Length(6, 7).WithMessage("Invalid Sort Code {0}", a => a.BankSortcode);

            RuleFor(a => a.DebitCardType).NotEmpty().WithMessage("Card Type required");

            RuleFor(a => a.DirectDeposit).NotEmpty().WithMessage("Payment Method required");

            RuleFor(a => a.BankAccountNumber)
                .NotEmpty().WithMessage("Account number required")
                .OnlyDigits().WithMessage("Invalid Account Number {0}", a => a.BankAccountNumber)
                .Length(8, 9).WithMessage("Invalid Account Number {0}", a => a.BankAccountNumber);


        }
       

    }
}
