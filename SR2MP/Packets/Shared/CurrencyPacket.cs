using SR2MP.Packets.Utils;

namespace SR2MP.Packets.Shared;

public struct CurrencyPacket : IPacket
{
    public readonly PacketType Type => PacketType.CurrencyAdjust;

    public int Adjust { get; set; }
    public byte CurrencyType { get; set; }
    public bool ShowUINotification { get; set; }

    public readonly void SerialiseTo(PacketWriter writer)
    {
        writer.WriteInt(Adjust);
        writer.WriteByte(CurrencyType);
        writer.WriteBool(ShowUINotification);
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        Adjust = reader.ReadInt();
        CurrencyType = reader.ReadByte();
        ShowUINotification = reader.ReadBool();
    }
}