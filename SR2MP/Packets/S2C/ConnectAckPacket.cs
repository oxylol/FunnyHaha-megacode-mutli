using SR2MP.Packets.Utils;

namespace SR2MP.Packets.S2C;

public sealed class ConnectAckPacket : IPacket
{
    public PacketType Type => PacketType.ConnectAck;

    public long PlayerId { get; private set; }
    public long[] OtherPlayers { get; private set; }

    public ConnectAckPacket() { }

    public ConnectAckPacket(long playerId, long[] otherPlayers)
    {
        PlayerId = playerId;
        OtherPlayers = otherPlayers;
    }

    public void SerialiseTo(PacketWriter writer)
    {
        writer.WriteLong(PlayerId);
        writer.WriteArray(OtherPlayers, (writer, val) => writer.WriteLong(val));
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        PlayerId = reader.ReadLong();
        OtherPlayers = reader.ReadArray(reader => reader.ReadLong());
    }
}