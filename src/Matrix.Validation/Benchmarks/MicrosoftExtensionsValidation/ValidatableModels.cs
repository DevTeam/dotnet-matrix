using Microsoft.Extensions.Validation;
// ReSharper disable CheckNamespace
namespace Matrix.Validation.Models;

#pragma warning disable ASP0029
[ValidatableType]
public sealed partial class BasicInput;

[ValidatableType]
public sealed partial class NestedInput;

[ValidatableType]
public sealed partial class CollectionInput;

[ValidatableType]
public sealed partial class ConditionalInput;

[ValidatableType]
public sealed partial class CustomInput;
#pragma warning restore ASP0029
