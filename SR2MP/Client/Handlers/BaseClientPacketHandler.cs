using SR2MP.Shared.Managers;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

public abstract class BaseClientPacketHandler : IClientPacketHandler
{
    protected readonly Client _client;
    protected readonly RemotePlayerManager _playerManager;

    protected BaseClientPacketHandler(Client client, RemotePlayerManager playerManager)
    {
        _client = client;
        _playerManager = playerManager;
    }

    public abstract void Handle(PacketReader reader);

    protected void SendPacket<TPacket>(TPacket packet) where TPacket : IPacket => _client.SendPacket(packet);
}