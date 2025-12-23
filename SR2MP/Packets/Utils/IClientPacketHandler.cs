namespace SR2MP.Packets.Utils;

public interface IClientPacketHandler
{
    void Handle(PacketReader reader);
}