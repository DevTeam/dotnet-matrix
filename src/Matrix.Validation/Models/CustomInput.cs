namespace Matrix.Validation.Models;

public sealed partial class CustomInput
{
    [EvenNumber]
    public int Code { get; set; }
}
