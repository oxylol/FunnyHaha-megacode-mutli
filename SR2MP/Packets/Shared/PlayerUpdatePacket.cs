using SR2MP.Packets.Utils;
using UnityEngine;

namespace SR2MP.Packets.Shared;

public struct PlayerUpdatePacket : IPacket
{
    public readonly PacketType Type => PacketType.PlayerUpdate;

    public long PlayerId { get; private set; }

    public Vector3 Position { get; private set; }

    public int AirborneState { get; private set; }

    public float Yaw { get; private set; }
    public float Rotation { get; private set; }
    public float LookY { get; private set; }
    public float HorizontalMovement { get; private set; }
    public float ForwardMovement { get; private set; }
    public float HorizontalSpeed { get; private set; }
    public float ForwardSpeed { get; private set; }

    public bool Moving { get; private set; }
    public bool Sprinting { get; private set; }

    public PlayerUpdatePacket(
        long playerId,
        Vector3 position,
        int airborneState,
        float yaw,
        float rotation,
        float lookY,
        float horizontalMovement,
        float forwardMovement,
        float horizontalSpeed,
        float forwardSpeed,
        bool moving,
        bool sprinting
    )
    {
        PlayerId = playerId;
        Position = position;
        AirborneState = airborneState;
        Yaw = yaw;
        Rotation = rotation;
        LookY = lookY;
        HorizontalMovement = horizontalMovement;
        ForwardMovement = forwardMovement;
        HorizontalSpeed = horizontalSpeed;
        ForwardSpeed = forwardSpeed;
        Moving = moving;
        Sprinting = sprinting;
    }

    public readonly void SerialiseTo(PacketWriter writer)
    {
        writer.WriteLong(PlayerId);

        writer.WriteVector3(Position);

        writer.WriteInt(AirborneState);

        writer.WriteFloat(Yaw);
        writer.WriteFloat(Rotation);
        writer.WriteFloat(LookY);
        writer.WriteFloat(HorizontalMovement);
        writer.WriteFloat(ForwardMovement);
        writer.WriteFloat(HorizontalSpeed);
        writer.WriteFloat(ForwardSpeed);

        writer.ResetPackingBools();
        writer.WritePackedBool(Moving);
        writer.WritePackedBool(Sprinting);
        writer.EndPackingBools();
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        PlayerId = reader.ReadLong();

        Position = reader.ReadVector3();

        AirborneState = reader.ReadInt();

        Yaw = reader.ReadFloat();
        Rotation = reader.ReadFloat();
        LookY = reader.ReadFloat();
        HorizontalMovement = reader.ReadFloat();
        ForwardMovement = reader.ReadFloat();
        HorizontalSpeed = reader.ReadFloat();
        ForwardSpeed = reader.ReadFloat();

        Moving = reader.ReadPackedBool();
        Sprinting = reader.ReadPackedBool();
        reader.EndPackingBools();
    }
}