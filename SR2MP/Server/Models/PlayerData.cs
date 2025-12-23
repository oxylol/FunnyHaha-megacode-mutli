using UnityEngine;

namespace SR2MP.Server.Models;

public sealed class PlayerData
{
    public long PlayerId { get; set; }
    public string PlayerName { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public PlayerData(long playerId = -1, string playerName = "Undefined name", Vector3 position = default, Quaternion rotation = default)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        Position = position;
        Rotation = rotation;
    }
}
