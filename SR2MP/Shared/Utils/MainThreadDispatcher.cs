using UnityEngine;
using System.Collections.Concurrent;
using System;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;

namespace SR2MP.Shared.Utils;

[RegisterTypeInIl2Cpp(false)]
public class MainThreadDispatcher : MonoBehaviour
{
    public static MainThreadDispatcher Instance { get; private set; }
    private static readonly ConcurrentQueue<Action> actionQueue = new();

    public static void Initialize()
    {
        if (Instance != null) return;

        var obj = new GameObject("SR2MP_MainThreadDispatcher");
        Instance = obj.AddComponent<MainThreadDispatcher>();
        DontDestroyOnLoad(obj);

        SrLogger.LogMessage("Main thread dispatcher initialized", SrLogger.LogTarget.Both);
    }

    void Update()
    {
        while (actionQueue.TryDequeue(out var action))
        {
            try
            {
                action?.Invoke();
                SrLogger.LogMessage($"Received some action: {action}", SrLogger.LogTarget.Both);
            }
            catch (Exception ex)
            {
                SrLogger.LogError($"Error executing main thread action: {ex}", SrLogger.LogTarget.Both);
            }
        }
    }

    public static void Enqueue(Action action)
    {
        actionQueue.Enqueue(action);
        SrLogger.LogMessage($"Enqueued some action: {action}");
    }

    private void OnDestroy()
    {
#nullable disable
        Instance = null;
#nullable enable
    }
}