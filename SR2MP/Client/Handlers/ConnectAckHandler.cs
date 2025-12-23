using SR2MP.Client.Managers;
using SR2MP.Components;
using SR2MP.Packets.C2S;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.ConnectAck)]
public sealed class ConnectAckHandler : BaseClientPacketHandler
{
    public ConnectAckHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager)
    {
    }

    public override void Handle(PacketReader reader)
    {
        var packet = reader.ReadNetObject<ConnectAckPacket>();

        var joinPacket = new PlayerJoinPacket(packet.PlayerId, "username");

        SendPacket(joinPacket);

        _client.StartHeartbeat();
        _client.NotifyConnected();

        SrLogger.LogMessage($"Connection acknowledged by server! (PlayerId: {packet.PlayerId})",
            SrLogger.LogTarget.Both);

        foreach (var player in packet.OtherPlayers)
        {
            SpawnPlayer(player);
        }
    }

    private static void SpawnPlayer(long id)
    {
        var playerObject = Object.Instantiate(playerPrefab).GetComponent<NetworkPlayer>();
        playerObject.gameObject.SetActive(true);
        playerObject.ID = id;
        playerObject.gameObject.name = id.ToString();
        playerObjects.Add(id, playerObject.gameObject);
        playerManager.AddPlayer(id);
        Object.DontDestroyOnLoad(playerObject);
    }
}