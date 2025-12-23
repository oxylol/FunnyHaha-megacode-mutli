using SR2MP.Packets.Utils;

namespace SR2MP.Packets.C2S;

public sealed class ClientHandshakePacket : IPacket
{
    public PacketType Type => PacketType.ClientHandshake;

    public string ModHash { get; private set; } = string.Empty;

    public ClientHandshakePacket() { }

    public ClientHandshakePacket(string modHash) => ModHash = modHash ?? string.Empty;

    public void SerialiseTo(PacketWriter writer) => writer.WriteString(ModHash);

    public void DeserialiseFrom(PacketReader reader) => ModHash = reader.ReadString();
}