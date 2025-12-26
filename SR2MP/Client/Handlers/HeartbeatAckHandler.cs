using SR2MP.Client.Managers;
using SR2MP.Shared.Managers;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[ClientPacketHandler((byte)PacketType.HeartbeatAck)]
public class HeartbeatAckHandler : BaseClientPacketHandler
{
    public HeartbeatAckHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager) { }

    public override void Handle(byte[] data)
    {
        // Heartbeat acknowledgment received - connection is alive
        // No action needed, just confirms server is responsive
    }
}
