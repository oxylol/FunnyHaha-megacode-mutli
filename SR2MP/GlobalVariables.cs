using SR2MP.Shared.Managers;
using UnityEngine;

namespace SR2MP;

public static class GlobalVariables
{
    internal static GameObject playerPrefab;

    public static Dictionary<long, GameObject> playerObjects = new();

    public static RemotePlayerManager playerManager = new RemotePlayerManager();

    public static RemoteFXManager fxManager = new RemoteFXManager();

    // To prevent stuff from being stuck in
    // an infinite sending loop qwq
    public static bool handlingPacket = false;

    // I love this indenting
    public static long LocalID =>
        Main.Server.IsRunning()
            ? 0
            : Main.Client.IsConnected
                ? Main.Client.OwnPlayerId
                : -1;
}