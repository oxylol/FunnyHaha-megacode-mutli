using MelonLoader;

namespace SR2MP;

public class Main : MelonMod
{
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        SR2MP.Logger.Log("test log owo :3");
    }
}