namespace Matrix;

public static class MatrixFeatureOrders
{
    /// <summary>
    /// The order at which deployment capabilities begin.
    /// </summary>
    /// <remarks>
    /// A scenario is a measured invocation and its order is contiguous from 1. A deployment
    /// capability, such as Native AOT compatibility, is a property of a build instead: it is
    /// validated rather than measured, it never reaches a benchmark report, and so it can never
    /// enter the rating. Keeping it above this threshold lets a reader, a report and the web
    /// application tell the two apart by order alone, and leaves room for a category to grow its
    /// scenarios without colliding.
    /// </remarks>
    public const int Deployment = 1000;
}
