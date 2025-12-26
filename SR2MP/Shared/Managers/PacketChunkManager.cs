using SR2MP.Packets.Utils;

namespace SR2MP.Shared.Managers;

public static class PacketChunkManager
{
    internal class IncompletePacket
    {
        public byte[][] chunks;
        public byte chunkIndex;
        public byte totalChunks;
    }
    internal static Dictionary<PacketType, IncompletePacket> incompletePackets = new();

    internal const int MaxChunkBytes = 250;

    internal static bool TryMergePacket(PacketType packetType, byte[] data, byte chunkIndex, byte totalChunks, out byte[] fullData)
    {
        fullData = null!;
        
        if (!incompletePackets.TryGetValue(packetType, out var packet))
        {
            packet = new IncompletePacket
            {
                chunks = new byte[totalChunks][],
                chunkIndex = 0,
                totalChunks = totalChunks,
            };
            incompletePackets[packetType] = packet;
        }
        
        packet.chunks[chunkIndex] = data;
        packet.chunkIndex++;
        SrLogger.LogPacketSize($"New chunk: type: {packetType}, index: {chunkIndex}, total: {totalChunks}");
        

        if (chunkIndex + 1 >= packet.totalChunks)
        {
            // Calculate total size and allocate single buffer
            var totalSize = 0;
            foreach (var chunk in packet.chunks)
            {
                totalSize += chunk.Length;
            }

            fullData = new byte[totalSize];
            var offset = 0;

            // Use Buffer.BlockCopy for efficient copying
            foreach (var chunk in packet.chunks)
            {
                Buffer.BlockCopy(chunk, 0, fullData, offset, chunk.Length);
                offset += chunk.Length;
            }

            incompletePackets.Remove(packetType);

            SrLogger.LogPacketSize($"Fully finished merge: type={packetType}");

            return true;
        }
        else
            return false;
    }

    internal static byte[][] SplitPacket(byte[] data)
    {
        var chunkCount = (data.Length + MaxChunkBytes - 1) / MaxChunkBytes;

        var packetType = data[0];
        var result = new byte[chunkCount][];
        for (byte index = 0; index < chunkCount; index++)
        {
            var offset = index * MaxChunkBytes;
            var chunkSize = Math.Min(MaxChunkBytes, data.Length - offset);
            
            var buffer = new byte[3 + chunkSize];
            buffer[0] = packetType;
            buffer[1] = index;
            buffer[2] = (byte)chunkCount;

            Buffer.BlockCopy(data, offset, buffer, 3, chunkSize);
            result[index] = buffer;
        }
        return result;
    }
}