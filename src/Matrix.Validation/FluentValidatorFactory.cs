using FluentValidation;

namespace Matrix.Validation;

internal static class FluentValidatorFactory
{
    public static InlineValidator<BasicInput> Basic(
        CascadeMode cascadeMode = CascadeMode.Continue)
    {
        var validator = new InlineValidator<BasicInput>
        {
            ClassLevelCascadeMode = cascadeMode
        };
        validator.RuleFor(input => input.Name).NotEmpty();
        validator.RuleFor(input => input.Email).EmailAddress();
        validator.RuleFor(input => input.Age).InclusiveBetween(18, 120);
        return validator;
    }

    public static InlineValidator<NestedInput> Nested()
    {
        var address = new InlineValidator<AddressInput>();
        address.RuleFor(input => input.Street).NotEmpty();
        address.RuleFor(input => input.PostalCode).NotEmpty();
        var validator = new InlineValidator<NestedInput>();
        validator.RuleFor(input => input.Address).NotNull().SetValidator(address);
        return validator;
    }

    public static InlineValidator<CollectionInput> Collection()
    {
        var item = new InlineValidator<LineItemInput>();
        item.RuleFor(input => input.Sku).NotEmpty();
        item.RuleFor(input => input.Quantity).InclusiveBetween(1, 1000);
        var validator = new InlineValidator<CollectionInput>();
        validator.RuleFor(input => input.Items).NotNull();
        validator.RuleForEach(input => input.Items).SetValidator(item);
        return validator;
    }

    public static InlineValidator<ConditionalInput> Conditional()
    {
        var validator = new InlineValidator<ConditionalInput>();
        validator
            .RuleFor(input => input.TaxId)
            .NotEmpty()
            .When(input => input.IsBusiness);
        return validator;
    }

    public static InlineValidator<CustomInput> Custom()
    {
        var validator = new InlineValidator<CustomInput>();
        validator.RuleFor(input => input.Code).Must(code => code % 2 == 0);
        return validator;
    }

    public static InlineValidator<AsyncInput> Async()
    {
        var validator = new InlineValidator<AsyncInput>();
        validator.RuleFor(input => input.UserName).MustAsync(
            async (userName, _) =>
            {
                await Task.Yield();
                return !string.Equals(userName, "taken", StringComparison.Ordinal);
            });
        return validator;
    }
}
