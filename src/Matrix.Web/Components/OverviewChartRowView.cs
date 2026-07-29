using Matrix;

namespace Matrix.Web.Components;

internal sealed record OverviewChartRowView(
    MatrixOverviewRow Row,
    string? Rank,
    string? Coverage,
    string? Place);
