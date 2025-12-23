using SR2MP.Packets.Utils;

namespace SR2MP.Packets.C2S;

public sealed class ChatMessagePacket : IPacket
{
    public PacketType Type => PacketType.ChatMessage;

    public long PlayerId { get; private set; }
    public string Message { get; private set; } = string.Empty;

    public ChatMessagePacket() { }

    public ChatMessagePacket(long playerId, string message)
    {
        PlayerId = playerId;
        Message = message;
    }

    public void SerialiseTo(PacketWriter writer)
    {
        writer.WriteLong(PlayerId);
        writer.WriteString(Message, 200, true);
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        PlayerId = reader.ReadLong();
        Message = reader.ReadString();
    }
}