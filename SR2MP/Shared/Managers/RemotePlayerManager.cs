using System.Collections.Concurrent;
using SR2MP.Client.Models;
using UnityEngine;

namespace SR2MP.Shared.Managers;

public sealed class RemotePlayerManager
{
    private readonly ConcurrentDictionary<long, RemotePlayer> players = new();

    public event Action<long>? OnPlayerAdded;
    public event Action<long>? OnPlayerRemoved;
    public event Action<long, RemotePlayer>? OnPlayerUpdated;

    public int PlayerCount => players.Count;

    public RemotePlayer GetOrAddPlayer(long playerId) => GetPlayer(playerId) ?? AddPlayer(playerId);

    public RemotePlayer? GetPlayer(long playerId)
    {
        players.TryGetValue(playerId, out var player);
        return player;
    }

    public RemotePlayer AddPlayer(long playerId)
    {
        var player = new RemotePlayer(playerId);

        if (players.TryAdd(playerId, player))
        {
            SrLogger.LogMessage($"Remote player added: {playerId}", SrLogger.LogTarget.Both);
            OnPlayerAdded?.Invoke(playerId);
            return player;
        }
        else
        {
            SrLogger.LogWarning($"Remote player already exists: {playerId}", SrLogger.LogTarget.Both);
            return players[playerId];
        }
    }

    public bool RemovePlayer(long playerId)
    {
        if (!players.TryRemove(playerId, out var player))
            return false;

        SrLogger.LogMessage($"Remote player removed: {playerId}", SrLogger.LogTarget.Both);
        OnPlayerRemoved?.Invoke(playerId);
        return true;
    }

    public static void SendPlayerUpdate(
        Vector3 position,
        float rotation,
        float horizontalMovement = 0f,
        float forwardMovement = 0f,
        float yaw = 0f,
        int airborneState = 0,
        bool moving = false,
        float horizontalSpeed = 0f,
        float forwardSpeed = 0f,
        bool sprinting = false,
        float lookY = 0f)
    {
        // I dont know.
        var playerId = Main.Client.IsConnected ? Main.Client.OwnPlayerId : (Main.Server.IsRunning() ? 0 : -1); // 0 = Host, -1 = Invalid

        var updatePacket = new PlayerUpdatePacket(playerId, position, airborneState, yaw, rotation, lookY, horizontalMovement, forwardMovement, horizontalSpeed, forwardSpeed, moving, sprinting);
        Main.SendToAllOrServer(updatePacket);
    }

    public void UpdatePlayer(
        long playerId,
        Vector3 position,
        float rotation,
        float horizontalMovement,
        float forwardMovement,
        float yaw,
        int airborneState,
        bool moving,
        float horizontalSpeed,
        float forwardSpeed,
        bool sprinting,
        float lookY)
    {
        if (!players.TryGetValue(playerId, out var player))
            return;

        player.Position = position;
        player.Rotation = rotation;
        player.HorizontalMovement = horizontalMovement;
        player.ForwardMovement = forwardMovement;
        player.Yaw = yaw;
        player.AirborneState = airborneState;
        player.Moving = moving;
        player.HorizontalSpeed = horizontalSpeed;
        player.ForwardSpeed = forwardSpeed;
        player.Sprinting = sprinting;
        player.LastLookY = player.LookY;
        player.LookY = lookY;
        player.LastUpdate = DateTime.UtcNow;
        OnPlayerUpdated?.Invoke(playerId, player);
    }

    public List<RemotePlayer> GetAllPlayers()
    {
        return players.Values.ToList();
    }

    public void Clear()
    {
        var allPlayers = players.Keys.ToList();
        players.Clear();

        foreach (var playerId in allPlayers)
        {
            OnPlayerRemoved?.Invoke(playerId);
        }

        SrLogger.LogMessage("All remote players cleared!", SrLogger.LogTarget.Both);
    }
}