using SR2MP.Shared.Managers;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.Close)]
public sealed class CloseHandler : BaseClientPacketHandler
{
    public CloseHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager) { }

    public override void Handle(PacketReader reader)
    {
        SrLogger.LogMessage("Server closed, disconnecting!", SrLogger.LogTarget.Both);
        _client.Disconnect();
    }
}