using SR2MP.Packets.Utils;

namespace SR2MP.Packets.Shared;

public struct CurrencyPacket : IPacket
{
    public readonly PacketType Type => PacketType.CurrencyAdjust;

    public int Adjust { get; private set; }
    public byte CurrencyType { get; private set; }
    public bool ShowUINotification { get; private set; }

    public CurrencyPacket(int adjust, byte currencyType, bool showUINotification)
    {
        Adjust = adjust;
        CurrencyType = currencyType;
        ShowUINotification = showUINotification;
    }

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