using MemoryPack;
using MessagePack;
using ProtoBuf;

namespace Matrix.BinarySerialization.Models;

[MessagePackObject]
[ProtoContract]
[MemoryPackable]
public partial class CustomerModel
{
    [Key(0)]
    [ProtoMember(1)]
    [MemoryPackOrder(0)]
    public string Name { get; set; } = string.Empty;

    [Key(1)]
    [ProtoMember(2)]
    [MemoryPackOrder(1)]
    public AddressModel Address { get; set; } = new();
}
