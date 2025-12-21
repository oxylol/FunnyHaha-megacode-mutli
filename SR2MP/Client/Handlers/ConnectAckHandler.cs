using SR2MP.Client.Managers;
using SR2MP.Components;
using SR2MP.Packets.C2S;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.ConnectAck)]
public class ConnectAckHandler : BaseClientPacketHandler
{
    public ConnectAckHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager)
    {
    }

    public override void Handle(byte[] data)
    {
        using var reader = new PacketReader(data);
        var packet = reader.ReadPacket<ConnectAckPacket>();

        var joinPacket = new PlayerJoinPacket
        {
            Type = (byte)PacketType.PlayerJoin, PlayerId = packet.PlayerId, PlayerName = "username"
        };

        SendPacket(joinPacket);

        Client.StartHeartbeat();
        Client.NotifyConnected();

        SrLogger.LogMessage($"Connection acknowledged by server! (PlayerId: {packet.PlayerId})",
            SrLogger.LogTarget.Both);
        
        var playerObject = Object.Instantiate(playerPrefab).GetComponent<NetworkPlayer>();
        playerObject.gameObject.SetActive(true);
        // Temporary solution becaus no fix :3
        playerObject.ID = "HOST";
        playerObject.gameObject.name = "HOST";
        playerObjects.Add("HOST", playerObject.gameObject);
        playerManager.AddPlayer("HOST");
        Object.DontDestroyOnLoad(playerObject);
    }
}