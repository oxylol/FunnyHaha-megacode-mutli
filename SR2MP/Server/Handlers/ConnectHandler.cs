using System.Net;
using SR2MP.Server.Managers;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.Connect)]
public sealed class ConnectHandler : BasePacketHandler
{
    public ConnectHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager)
    {
    }

    public override void Handle(PacketReader reader, IPEndPoint senderEndPoint)
    {
        var playerId = reader.ReadLong();

        SrLogger.LogMessage($"Connect request received with PlayerId: {playerId}",
            $"Connect request from {senderEndPoint} with PlayerId: {playerId}");

        var client = clientManager.AddClient(senderEndPoint, playerId);

        var ackPacket = new ConnectAckPacket(playerId, Array.ConvertAll(playerManager.GetAllPlayers().ToArray(), input => input.PlayerId));

        SendToClient(ackPacket, client);

        var joinPacket = new BroadcastPlayerJoinPacket(playerId);

        BroadcastToAllExcept(joinPacket, senderEndPoint);

        SrLogger.LogMessage($"Player {playerId} successfully connected",
            $"Player {playerId} successfully connected from {senderEndPoint}");
    }
}