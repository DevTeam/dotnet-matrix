using MemoryPack;
using MessagePack;
using ProtoBuf;

namespace Matrix.BinarySerialization.Models;

[MessagePackObject]
[ProtoContract]
[MemoryPackable]
public partial class EnumModel
{
    [Key(0)]
    [ProtoMember(1)]
    [MemoryPackOrder(0)]
    public ProcessingStatus Status { get; set; }
}
