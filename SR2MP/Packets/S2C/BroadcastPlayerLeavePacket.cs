using SR2MP.Packets.Utils;

namespace SR2MP.Packets.S2C;

public struct BroadcastPlayerLeavePacket : IPacket
{
    public readonly PacketType Type => PacketType.BroadcastPlayerLeave;

    public long PlayerId { get; private set; }

    public BroadcastPlayerLeavePacket(long playerId) => PlayerId = playerId;

    public readonly void SerialiseTo(PacketWriter writer) => writer.WriteLong(PlayerId);

    public void DeserialiseFrom(PacketReader reader) => PlayerId = reader.ReadLong();
}