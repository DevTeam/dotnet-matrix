using MemoryPack;
using MessagePack;
using ProtoBuf;

namespace Matrix.BinarySerialization.Models;

[MessagePackObject]
[ProtoContract]
[MemoryPackable]
public partial class SimpleModel
{
    [Key(0)]
    [ProtoMember(1)]
    [MemoryPackOrder(0)]
    public int Id { get; set; }

    [Key(1)]
    [ProtoMember(2)]
    [MemoryPackOrder(1)]
    public string Name { get; set; } = string.Empty;

    [Key(2)]
    [ProtoMember(3)]
    [MemoryPackOrder(2)]
    public bool Active { get; set; }
}
