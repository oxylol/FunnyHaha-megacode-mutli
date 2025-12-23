using SR2MP.Client.Managers;
using SR2MP.Packets.C2S;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.PlayerUpdate)]
public sealed class PlayerUpdateHandler : BaseClientPacketHandler
{
    public PlayerUpdateHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager)
    {
    }

    public override void Handle(PacketReader reader)
    {
        var packet = reader.ReadNetObject<PlayerUpdatePacket>();

        // Don't update our own player
        if (packet.PlayerId == _client.OwnPlayerId)
            return;

        _playerManager.UpdatePlayer(
            packet.PlayerId,
            packet.Position,
            packet.Rotation,
            packet.HorizontalMovement,
            packet.ForwardMovement,
            packet.Yaw,
            packet.AirborneState,
            packet.Moving,
            packet.HorizontalSpeed,
            packet.ForwardSpeed,
            packet.Sprinting
        );
    }
}