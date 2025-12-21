using SR2MP.Packets.Utils;

namespace SR2MP.Packets.C2S;

// We should make the PlayerId come from the endpoint of the sender, if possible

public struct PlayerJoinPacket : IPacket
{
    public byte Type { get; set; }
    public string PlayerId { get; set; }
    public string PlayerName { get; set; }

    public readonly void Serialise(PacketWriter writer)
    {
        writer.WriteByte(Type);
        writer.WriteString(PlayerId);
        writer.WriteString(PlayerName);
    }

    public void Deserialise(PacketReader reader)
    {
        Type = reader.ReadByte();
        PlayerId = reader.ReadString();
        PlayerName = reader.ReadString();
    }
}