using HarmonyLib;
using Il2Cpp;

namespace SR2MP.Patches.Time;

[HarmonyPatch(typeof(TimeDirector), nameof(TimeDirector.FastForwardTo))]
public static class OnFastForward
{
    public static void Postfix(TimeDirector __instance, double fastForwardUntil)
    {
        if (handlingPacket)
            return;

        if (Main.Server.IsRunning())
        {
            var packet = new WorldTimePacket(fastForwardUntil);

            Main.Server.SendToAll(packet);
        }
        else if (Main.Client.IsConnected)
        {
            var packet = new WorldTimePacket(fastForwardUntil);

            Main.Client.SendPacket(packet);
        }
    }
}