using SR2MP.Packets.Utils;

namespace SR2MP.Packets.Shared;

public struct WorldTimePacket : IPacket
{
    public readonly PacketType Type => PacketType.WorldTime;

    public double Time { get; private set; }

    public WorldTimePacket(double time) => Time = time;

    public readonly void SerialiseTo(PacketWriter writer) => writer.WriteDouble(Time);

    public void DeserialiseFrom(PacketReader reader) => Time = reader.ReadDouble();
}