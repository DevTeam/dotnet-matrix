using System.Collections.Generic;
using MemoryPack;
using MessagePack;
using ProtoBuf;

namespace Matrix.BinarySerialization.Models;

[MessagePackObject]
[ProtoContract]
[MemoryPackable]
public partial class CollectionModel
{
    [Key(0)]
    [ProtoMember(1)]
    [MemoryPackOrder(0)]
    public List<SimpleModel> Items { get; set; } = [];
}
