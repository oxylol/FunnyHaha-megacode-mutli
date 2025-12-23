namespace SR2MP.Packets.Utils;

public interface INetObject
{
    void SerialiseTo(PacketWriter writer);

    void DeserialiseFrom(PacketReader reader);
}