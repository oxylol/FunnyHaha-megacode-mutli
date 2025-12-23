using SR2MP.Packets.Utils;

namespace SR2MP.Packets.S2C;

public sealed class BroadcastChatMessagePacket : IPacket
{
    public PacketType Type => PacketType.BroadcastChatMessage;

    public long PlayerId { get; private set; }
    public long Timestamp { get; private set; }
    public string Message { get; private set; }

    public BroadcastChatMessagePacket() { }

    public BroadcastChatMessagePacket(long playerId, long timestamp, string message)
    {
        PlayerId = playerId;
        Timestamp = timestamp;
        Message = message;
    }

    public void SerialiseTo(PacketWriter writer)
    {
        writer.WriteLong(PlayerId);
        writer.WriteLong(Timestamp);
        writer.WriteString(Message);
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        PlayerId = reader.ReadLong();
        Timestamp = reader.ReadLong();
        Message = reader.ReadString();
    }
}