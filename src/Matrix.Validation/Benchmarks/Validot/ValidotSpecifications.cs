using Validot;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

internal static class ValidotSpecifications
{
    public static IValidator<BasicInput> Basic() =>
        ValidotValidator.Factory.Create<BasicInput>(
            input => input
                .Member(m => m.Name, name => name.NotEmpty())
                .Member(m => m.Email, email => email.Email())
                .Member(m => m.Age, age => age.BetweenOrEqualTo(18, 120)));

    public static IValidator<NestedInput> Nested() =>
        ValidotValidator.Factory.Create<NestedInput>(
            input => input.Member(m => m.Address, address => address
                .Member(a => a.Street, street => street.NotEmpty())
                .Member(a => a.PostalCode, postal => postal.NotEmpty())));

    public static IValidator<ConditionalInput> Conditional() =>
        ValidotValidator.Factory.Create<ConditionalInput>(
            input => input
                .Member(m => m.TaxId, tax => tax.NotEmpty())
                .WithCondition(input => input.IsBusiness));

    public static IValidator<CustomInput> Custom() =>
        ValidotValidator.Factory.Create<CustomInput>(
            input => input.Member(m => m.Code, code => code.Rule(value => value % 2 == 0)));
}
