using HarmonyLib;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Player.PlayerItems;

namespace SR2MP.Patches.FX;

[HarmonyPatch(typeof(VacuumItem), nameof(VacuumItem.PlayTransientAudio))]
public static class OnPlayVacAudio
{   //                                                  dont rename 'cue', breaks everything
    public static void Postfix(SECTR_AudioCue cue, float volume = 1f) // cant change volume either
    {
        SendPacket(cue);
    }
    static void SendPacket(SECTR_AudioCue cue)
    {
        // If the audio cue isn't in the player dictionary
        if (!fxManager.TryGetFXType(cue, out var fxType))
        {
            return;
        }

        var packet = new PlayerFXPacket(fxType, LocalID);

        Main.SendToAllOrServer(packet);
    }
}