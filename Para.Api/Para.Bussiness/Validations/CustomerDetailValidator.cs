using FluentValidation;
using Para.Schema;

namespace Para.Bussiness.Validations
{
    public class CustomerDetailValidator : AbstractValidator<CustomerDetailRequest>
    {
        public CustomerDetailValidator()
        {
            RuleFor(x => x.FatherName)
                .NotEmpty().WithMessage("Father Name is required.");

            RuleFor(x => x.MotherName)
                .NotEmpty().WithMessage("Mother Name is required.");

            RuleFor(x => x.EducationStatus)
                .NotEmpty().WithMessage("Education Status is required.");

            RuleFor(x => x.MontlyIncome)
                .NotEmpty().WithMessage("Monthly Income is required.");

            RuleFor(x => x.Occupation)
                .NotEmpty().WithMessage("Occupation is required.");
        }
    }
}
