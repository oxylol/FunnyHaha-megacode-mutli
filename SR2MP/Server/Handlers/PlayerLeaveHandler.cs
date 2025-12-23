using System.Net;
using SR2MP.Server.Managers;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.PlayerLeave)]
public sealed class PlayerLeaveHandler : BasePacketHandler
{
    public PlayerLeaveHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager) { }

    public override void Handle(PacketReader reader, IPEndPoint senderEndPoint)
    {
        var playerId = reader.ReadLong();

        var clientInfo = $"{senderEndPoint.Address}:{senderEndPoint.Port}";

        SrLogger.LogMessage($"Player leave request received (PlayerId: {playerId})",
            $"Player leave request from {clientInfo} (PlayerId: {playerId})");

        if (clientManager.RemoveClient(clientInfo))
        {
            var leavePacket = new BroadcastPlayerLeavePacket(playerId);

            BroadcastToAll(leavePacket);

            SrLogger.LogMessage($"Player {playerId} left the server",
                $"Player {playerId} left from {clientInfo}");
        }
        else
        {
            SrLogger.LogWarning($"Player leave request from unknown client (PlayerId: {playerId})",
                $"Player leave request from unknown client: {clientInfo} (PlayerId: {playerId})");
        }
    }
}