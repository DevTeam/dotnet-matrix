namespace Matrix.CliParsing.Models;

/// <summary>
/// The result of a parse that is expected to fail. Both a thrown library exception
/// captured by the adapter and a returned diagnostic collection are represented here.
/// </summary>
public sealed record ParseOutcome(bool Succeeded, string Error);
