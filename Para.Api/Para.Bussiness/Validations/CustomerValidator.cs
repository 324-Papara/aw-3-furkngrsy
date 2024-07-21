using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations
{
    public class CustomerValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Length(2, 50).WithMessage("First Name must be between 2 and 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Length(2, 50).WithMessage("Last Name must be between 2 and 50 characters.");

            RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("Identity Number is required.")
                .Length(11).WithMessage("Identity Number must be 11 characters long.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");
        }
    }
}
