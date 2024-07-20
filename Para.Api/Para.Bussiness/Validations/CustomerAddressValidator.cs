using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations
{
    public class CustomerAddressValidator : AbstractValidator<CustomerAddressRequest>
    {
        public CustomerAddressValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.AddressLine)
                .NotEmpty().WithMessage("Address Line is required.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip Code is required.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("Is Default must be specified.");
        }
    }
}
