using SR2MP.Client.Managers;
using SR2MP.Packets.S2C;
using SR2MP.Packets.Utils;

namespace SR2MP.Client.Handlers;

[PacketHandler((byte)PacketType.BroadcastChatMessage)]
public sealed class ChatMessageHandler : BaseClientPacketHandler
{
    public ChatMessageHandler(Client client, RemotePlayerManager playerManager)
        : base(client, playerManager)
    {
    }

    public override void Handle(PacketReader reader)
    {
        var packet = reader.ReadNetObject<BroadcastChatMessagePacket>();

        var messageTime = DateTimeOffset.FromUnixTimeMilliseconds(packet.Timestamp).UtcDateTime;

        SrLogger.LogMessage($"[{packet.PlayerId}]: {packet.Message}",
            $"Chat message from {packet.PlayerId} at {messageTime}: {packet.Message}");

        _client.NotifyChatMessageReceived(packet.PlayerId, packet.Message, messageTime);
    }
}