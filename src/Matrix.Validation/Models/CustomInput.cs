namespace Matrix.Validation.Models;

public sealed class CustomInput
{
    [EvenNumber]
    public int Code { get; set; }
}
