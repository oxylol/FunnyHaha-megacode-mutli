using System.Net;
using Il2Cpp;
using SR2MP.Server.Managers;
using SR2MP.Packets.Utils;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.FastForward)]
public sealed class FastForwardHandler : BasePacketHandler
{
    public FastForwardHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager) { }

    public override void Handle(PacketReader reader, IPEndPoint senderEndPoint)
    {
        var packet = reader.ReadNetObject<WorldTimePacket>();

        handlingPacket = true;
        SceneContext.Instance.TimeDirector.FastForwardTo(packet.Time);
        handlingPacket = false;

        Main.Server.SendToAllExcept(new WorldTimePacket(packet.Time), senderEndPoint);
    }
}