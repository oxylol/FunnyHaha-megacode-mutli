using SR2MP.Packets.Utils;

namespace SR2MP.Packets.S2C;

public struct ClosePacket : IPacket
{
    public readonly PacketType Type => PacketType.Close;

    public readonly void SerialiseTo(PacketWriter writer) { }

    public readonly void DeserialiseFrom(PacketReader reader) { }
}