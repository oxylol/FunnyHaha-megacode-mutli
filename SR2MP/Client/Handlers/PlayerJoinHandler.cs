using SR2MP.Client.Managers;
using SR2MP.Components;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.BroadcastPlayerJoin)]
public sealed class PlayerJoinHandler : BaseClientPacketHandler
{
    public PlayerJoinHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager) { }

    public override void Handle(PacketReader reader)
    {
        var playerId = reader.ReadLong();

        _playerManager.AddPlayer(playerId);

        if (playerId.Equals(_client.OwnPlayerId))
        {
            SrLogger.LogMessage("Player join request accepted!", SrLogger.LogTarget.Both);
            return;
        }

        SrLogger.LogMessage($"New Player joined! (PlayerId: {playerId})", SrLogger.LogTarget.Both);

        var playerObject = Object.Instantiate(playerPrefab).GetComponent<NetworkPlayer>();
        playerObject.gameObject.SetActive(true);
        playerObject.ID = playerId;
        playerObject.gameObject.name = playerId.ToString();
        playerObjects.Add(playerId, playerObject.gameObject);
        Object.DontDestroyOnLoad(playerObject);
    }
}