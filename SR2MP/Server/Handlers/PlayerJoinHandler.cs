using System.Net;
using SR2MP.Components;
using SR2MP.Server.Managers;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.PlayerJoin)]
public sealed class PlayerJoinHandler : BasePacketHandler
{
    public PlayerJoinHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager) { }

    public override void Handle(PacketReader reader, IPEndPoint senderEndPoint)
    {
        var playerId = reader.ReadLong();

        var address = $"{senderEndPoint.Address}:{senderEndPoint.Port}";

        SrLogger.LogMessage($"Player join request received (PlayerId: {playerId})",
            $"Player join request from {address} (PlayerId: {playerId})");

        var playerObject = Object.Instantiate(playerPrefab).GetComponent<NetworkPlayer>();
        playerObject.gameObject.SetActive(true);
        playerObject.ID = playerId;
        playerObject.gameObject.name = playerId.ToString();
        playerObjects.Add(playerId, playerObject.gameObject);
        Object.DontDestroyOnLoad(playerObject);

        var joinPacket = new BroadcastPlayerJoinPacket(playerId);

        BroadcastToAll(joinPacket);
    }
}