using SR2MP.Packets.Utils;

namespace SR2MP.Packets.Shared;

public struct EmptyPacket : IPacket
{
    public readonly PacketType Type => PacketType.Heartbeat;

    public readonly void SerialiseTo(PacketWriter writer) { }

    public readonly void DeserialiseFrom(PacketReader reader) { }
}