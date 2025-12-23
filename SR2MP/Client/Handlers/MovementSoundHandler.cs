using SR2MP.Shared.Managers;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.MovementSound)]
public sealed class MovementSoundHandler : BaseClientPacketHandler
{
    public MovementSoundHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager) { }

    public override void Handle(PacketReader reader)
    {
        var packet = reader.ReadNetObject<MovementSoundPacket>();

        RemoteFXManager.PlayTransientAudio(fxManager.allCues[packet.CueName], packet.Position);
    }
}