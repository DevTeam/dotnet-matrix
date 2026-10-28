using MemoryPack;
using MessagePack;
using ProtoBuf;

namespace Matrix.BinarySerialization.Models;

[MessagePackObject]
[ProtoContract]
[MemoryPackable]
public partial class OrderModel
{
    [Key(0)]
    [ProtoMember(1)]
    [MemoryPackOrder(0)]
    public int Id { get; set; }

    [Key(1)]
    [ProtoMember(2)]
    [MemoryPackOrder(1)]
    public CustomerModel Customer { get; set; } = new();
}
