using ValidationModules;
using ValidationModulesContext = ValidationModules.ValidationContext;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Benchmarks;

internal sealed class ValidationModulesUserNameAvailability : IAsyncValidatorFor<AsyncInput>
{
    public async ValueTask ValidateAsync(
        ValidationModulesContext context,
        AsyncInput value,
        CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        if (string.Equals(value.UserName, "taken", StringComparison.Ordinal))
        {
            context.Report(
                nameof(value.UserName),
                "duplicate",
                "The user name is already taken.");
        }
    }
}
