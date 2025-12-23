using Il2CppTMPro;
using MelonLoader;
using SR2E.Expansion;
using SR2MP.Components;
using SR2MP.Shared.Utils;
using UnityEngine;

namespace SR2MP;

public sealed class Main : SR2EExpansionV3
{
    public static Client.Client Client { get; private set; }
    public static Server.Server Server { get; private set; }

    public static string Username => _prefs.GetEntry<string>("username").Value;

    private static MelonPreferences_Category _prefs;

    public override void OnLateInitializeMelon()
    {
        _prefs = MelonPreferences.CreateCategory("SR2MP");
        _prefs.CreateEntry("username", "Player");

        Client = new Client.Client();
        Server = new Server.Server();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        switch (sceneName)
        {
            case "SystemCore":
            {
                MainThreadDispatcher.Initialize();
                break;
            }
            case "MainMenuEnvironment":
            {
                playerPrefab = new GameObject("PLAYER");
                playerPrefab.SetActive(false);
                playerPrefab.transform.localScale = Vector3.one * 0.85f;

                var playerModel = Object.Instantiate(GameObject.Find("BeatrixMainMenu")).transform;
                playerModel.parent.SetParent(playerPrefab.transform);
                playerModel.localPosition = Vector3.zero;
                playerModel.localRotation = Quaternion.identity;
                playerModel.localScale = Vector3.one;

                var name = new GameObject("Username");
                name.transform.SetParent(playerPrefab.transform);
                name.transform.localPosition = Vector3.up * 3;

                playerPrefab.AddComponent<NetworkPlayer>().usernamePanel = name.AddComponent<TextMeshPro>();

                Object.DontDestroyOnLoad(playerPrefab);
                break;
            }
        }
    }
}