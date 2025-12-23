using System.Net;
using SR2MP.Packets.Utils;
using SR2MP.Server.Managers;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.PlayerUpdate)]
public sealed class PlayerUpdateHandler : BasePacketHandler
{
    public PlayerUpdateHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager) { }

    public override void Handle(PacketReader reader, IPEndPoint senderEndPoint)
    {
        var packet = reader.ReadNetObject<PlayerUpdatePacket>();

        // This is temporary :3
        if (packet.PlayerId == 0)
            return;

        playerManager.UpdatePlayer(
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
            packet.Sprinting,
            packet.LookY
        );

        Main.Server.SendToAllExcept(packet, senderEndPoint);
    }
}