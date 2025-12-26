using System.Net;
using SR2MP.Server.Managers;
using SR2MP.Packets.Utils;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.Heartbeat)]
public class HeartbeatHandler : BasePacketHandler
{
    public HeartbeatHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager) { }

    public override void Handle(byte[] data, IPEndPoint senderEndPoint)
    {
        // Update the client's heartbeat timestamp
        clientManager.UpdateHeartbeat(senderEndPoint);

        // Send acknowledgment back to client
        var ackPacket = new EmptyPacket
        {
            Type = (byte)PacketType.HeartbeatAck
        };

        Main.Server.SendToClient(ackPacket, senderEndPoint);
    }
}
