using FluentValidation;

namespace BuyRequest.Domain.Validators
{
    public class ProductRequestValidator : AbstractValidator<Entities.ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.ProductDescription)
             .NotEmpty().WithMessage("Product Description field is required");

            RuleFor(x => x.ProductCategory)
               .NotNull().WithMessage("Operation field is required")
               .IsInEnum().WithMessage("Invalid Product Category");

            RuleFor(x => x.Quantity)
               .NotNull().WithMessage("Product Quantity field is required")
               .GreaterThan(0).WithMessage("Product Quantity must be higher than 0");

            RuleFor(x => x.Pvp)
               .NotNull().WithMessage("Product Price field is required")
               .GreaterThan(0).WithMessage("Product Price must be higher than 0");

            RuleFor(x => x.Total)
               .NotNull().WithMessage("Total field is required")
               .GreaterThan(0).WithMessage("Total must be higher than 0");
        }
    }
}
