using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations
{
    public class CustomerPhoneValidator : AbstractValidator<CustomerPhoneRequest>
    {
        public CustomerPhoneValidator()
        {
            RuleFor(x => x.CountyCode)
                .NotEmpty().WithMessage("County Code is required.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("Is Default must be specified.");
        }
    }
}
