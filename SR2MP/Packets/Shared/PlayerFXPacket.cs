using SR2MP.Packets.Utils;
using UnityEngine;

namespace SR2MP.Packets.Shared;

public struct PlayerFXPacket : IPacket
{
    public enum PlayerFXType : byte
    {
        None,
        VacReject,
        VacHold,
        VacAccept,
        WalkTrail,
        VacShoot,
        VacShootEmpty,
        WaterSplash,
        VacSlotChange,
        VacRunning,
        VacRunningStart,
        VacRunningEnd,
        VacShootSound,
    }

    public readonly PacketType Type => PacketType.PlayerFX;

    public PlayerFXType FX { get; private set; }
    public Vector3 Position { get; private set; } // For visual stuff
    public long Player { get; private set; } // For sound only

    public PlayerFXPacket(PlayerFXType fx, long player) : this(fx, default, player) { }

    public PlayerFXPacket(PlayerFXType fx, Vector3 position) : this(fx, position, -1) { }

    public PlayerFXPacket(PlayerFXType fx, Vector3 position, long player)
    {
        FX = fx;
        Position = position;
        Player = player;
    }

    public readonly void SerialiseTo(PacketWriter writer)
    {
        writer.WriteEnum(FX);

        if (!IsPlayerSoundDictionary[FX])
            writer.WriteVector3(Position);
        else
            writer.WriteLong(Player);
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        FX = reader.ReadEnum<PlayerFXType>();

        if (!IsPlayerSoundDictionary[FX])
            Position = reader.ReadVector3();
        else
            Player = reader.ReadLong();
    }
}