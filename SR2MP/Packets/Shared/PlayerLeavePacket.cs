using SR2MP.Packets.Utils;

namespace SR2MP.Packets.Shared;

// We should make the PlayerId come from the endpoint of the sender, if possible
public struct PlayerLeavePacket : IPacket
{
    public readonly PacketType Type => PacketType.PlayerLeave;

    public long PlayerId { get; private set; }

    public PlayerLeavePacket(long playerId) => PlayerId = playerId;

    public readonly void SerialiseTo(PacketWriter writer) => writer.WriteLong(PlayerId);

    public void DeserialiseFrom(PacketReader reader) => PlayerId = reader.ReadLong();
}