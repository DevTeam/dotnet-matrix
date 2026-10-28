using MemoryPack;
using MessagePack;
using ProtoBuf;

namespace Matrix.BinarySerialization.Models;

[MessagePackObject]
[ProtoContract]
[MemoryPackable]
public partial class AddressModel
{
    [Key(0)]
    [ProtoMember(1)]
    [MemoryPackOrder(0)]
    public string City { get; set; } = string.Empty;

    [Key(1)]
    [ProtoMember(2)]
    [MemoryPackOrder(1)]
    public string PostalCode { get; set; } = string.Empty;
}
