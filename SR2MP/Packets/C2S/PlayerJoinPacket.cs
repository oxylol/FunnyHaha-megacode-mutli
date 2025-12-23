using SR2MP.Packets.Utils;

namespace SR2MP.Packets.C2S;

// We should make the PlayerId come from the endpoint of the sender, if possible

public sealed class PlayerJoinPacket : IPacket
{
    public PacketType Type => PacketType.PlayerJoin;

    public long PlayerId { get; private set; }
    public string PlayerName { get; private set; } = string.Empty;

    public PlayerJoinPacket() { }

    public PlayerJoinPacket(long playerId, string playerName)
    {
        PlayerId = playerId;
        PlayerName = playerName;
    }

    public void SerialiseTo(PacketWriter writer)
    {
        writer.WriteLong(PlayerId);
        writer.WriteString(PlayerName);
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        PlayerId = reader.ReadLong();
        PlayerName = reader.ReadString();
    }
}