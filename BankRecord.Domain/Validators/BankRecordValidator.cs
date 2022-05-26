using FluentValidation;

namespace BankRecord.Domain.Validators
{
    public class BankRecordValidator : AbstractValidator<Entities.BankRecord>
    {
        public BankRecordValidator()
        {
            RuleFor(x => x.Origin)
              .NotNull().WithMessage("Origin field is required")
              .IsInEnum().WithMessage("Invalid Origin");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("Type field is required")
                .IsInEnum().WithMessage("Invalid Type");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description field is required");

            RuleFor(x => x.Amount)
              .NotNull().WithMessage("Amount field is required")
              .GreaterThan(0).WithMessage("Amount must be positive!")
              .When(x => x.Type == Entities.Enums.Type.Receive, ApplyConditionTo.CurrentValidator)
              .LessThan(0).WithMessage("Amount must be negative!")
              .When(x => x.Type == Entities.Enums.Type.Payment, ApplyConditionTo.CurrentValidator);
        }
    }
}
