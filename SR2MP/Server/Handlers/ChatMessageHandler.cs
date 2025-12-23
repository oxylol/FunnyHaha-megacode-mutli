using System.Net;
using SR2MP.Server.Managers;
using SR2MP.Packets.C2S;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Server.Handlers;

[PacketHandler((byte)PacketType.ChatMessage)]
public sealed class ChatMessageHandler : BasePacketHandler
{
    public ChatMessageHandler(NetworkManager networkManager, ClientManager clientManager)
        : base(networkManager, clientManager)
    {
    }

    public override void Handle(PacketReader reader, IPEndPoint senderEndPoint)
    {
        var packet = reader.ReadNetObject<ChatMessagePacket>();

        SrLogger.LogMessage($"Chat message from {packet.PlayerId}: {packet.Message}",
            $"Chat message from {senderEndPoint} ({packet.PlayerId}): {packet.Message}");

        var broadcastPacket = new BroadcastChatMessagePacket(packet.PlayerId, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), packet.Message);

        // Broadcast to self for confirmation (if a GUI will exist later qwq)
        // If necessary, not sure how we should do it
        BroadcastToAll(broadcastPacket);
    }
}