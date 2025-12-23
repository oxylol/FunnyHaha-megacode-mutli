using SR2MP.Packets.Utils;

namespace SR2MP.Packets.S2C;

public struct BroadcastPlayerJoinPacket : IPacket
{
    public readonly PacketType Type => PacketType.BroadcastPlayerJoin;

    public long PlayerId { get; private set; }

    public BroadcastPlayerJoinPacket(long playerId) => PlayerId = playerId;

    public readonly void SerialiseTo(PacketWriter writer) => writer.WriteLong(PlayerId);

    public void DeserialiseFrom(PacketReader reader) => PlayerId = reader.ReadLong();
}