using SR2MP.Packets.Utils;

namespace SR2MP.Packets.C2S;

public struct ConnectPacket : IPacket
{
    public readonly PacketType Type => PacketType.Connect;

    public long PlayerId { get; private set; }

    public ConnectPacket(long playerId) => PlayerId = playerId;

    public readonly void SerialiseTo(PacketWriter writer) => writer.WriteLong(PlayerId);

    public void DeserialiseFrom(PacketReader reader) => PlayerId = reader.ReadLong();
}