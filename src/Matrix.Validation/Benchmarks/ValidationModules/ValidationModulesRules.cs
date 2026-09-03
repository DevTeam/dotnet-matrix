using ValidationModules;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

public sealed class ValidationModulesRules :
    IValidationRulesFor<BasicInput>,
    IValidationRulesFor<AddressInput>,
    IValidationRulesFor<NestedInput>,
    IValidationRulesFor<LineItemInput>,
    IValidationRulesFor<CollectionInput>,
    IValidationRulesFor<ConditionalInput>,
    IValidationRulesFor<CustomInput>
{
    public static void Describe(ValidationRules<BasicInput> rules, BasicInput x)
    {
        rules.Require(x.Name);
        rules.Ensure(ConstraintChecks.IsEmail(x.Email), code: ValidationCodes.Email);
        rules.Range(x.Age, 18, 120);
    }

    public static void Describe(ValidationRules<AddressInput> rules, AddressInput x)
    {
        rules.Require(x.Street);
        rules.Require(x.PostalCode);
    }

    public static void Describe(ValidationRules<NestedInput> rules, NestedInput x) =>
        rules.Nested(x.Address);

    public static void Describe(ValidationRules<LineItemInput> rules, LineItemInput x)
    {
        rules.Require(x.Sku);
        rules.Range(x.Quantity, 1, 1000);
    }

    public static void Describe(ValidationRules<CollectionInput> rules, CollectionInput x) =>
        rules.Each(x.Items);

    public static void Describe(ValidationRules<ConditionalInput> rules, ConditionalInput x)
    {
        if (x.IsBusiness)
        {
            rules.Require(x.TaxId);
        }
    }

    public static void Describe(ValidationRules<CustomInput> rules, CustomInput x) =>
        rules.Ensure(x.Code % 2 == 0, code: "even");
}
