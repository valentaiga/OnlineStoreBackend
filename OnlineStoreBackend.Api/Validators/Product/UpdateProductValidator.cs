using FluentValidation;
using OnlineStoreBackend.Api.Models.Product;

namespace OnlineStoreBackend.Api.Validators.Product;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Name is empty");
        RuleFor(x => x.Name).Length(3, 100)
            .WithMessage("Name length must be greater than 3 and less than 100 characters");
        RuleFor(x => x.Path).Length(3, 50).When(x => x.Path is not null)
            .WithMessage("Path length must be greater than 3 and less than 50 characters");
        RuleFor(x => x.Description).Length(3, 500).When(x => x.Description is not null)
            .WithMessage("Description length must be greater than 3 and less than 500 characters");
        RuleFor(x => x.Code).Must(ValidateCode)
            .WithMessage("Code is not valid. It should contain numbers and '_' only");
    }

    private static bool ValidateCode(string code)
        => Constants.ProductCodeRegex.IsMatch(code);
}