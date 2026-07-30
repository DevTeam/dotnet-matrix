using System.Text;

namespace Matrix;

[AttributeUsage(AttributeTargets.Method)]
public sealed class PayloadSizeAttribute(string utf8Payload) : Attribute
{
    public int Bytes { get; } = Encoding.UTF8.GetByteCount(utf8Payload);
}
