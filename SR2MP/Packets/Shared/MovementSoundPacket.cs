using SR2MP.Packets.Utils;
using UnityEngine;

namespace SR2MP.Packets.Shared;

// Do not rewrite this, the movement SFX comes in many materials and types (36 different sounds).
// Il2CppMonomiPark.SlimeRancher.VFX.EnvironmentInteraction;
// GroundCollisionMaterials.GroundCollisionMaterialType.X
public sealed class MovementSoundPacket : IPacket
{
    public PacketType Type => PacketType.MovementSound;

    public Vector3 Position { get; set; }
    public string CueName { get; set; }

    public void SerialiseTo(PacketWriter writer)
    {
        writer.WriteVector3(Position);
        writer.WriteString(CueName);
    }

    public void DeserialiseFrom(PacketReader reader)
    {
        Position = reader.ReadVector3();
        CueName = reader.ReadString();
    }
}