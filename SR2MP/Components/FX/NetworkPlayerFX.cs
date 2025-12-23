using MelonLoader;
using UnityEngine;

namespace SR2MP.Components.FX;

[RegisterTypeInIl2Cpp(false)]
public sealed class NetworkPlayerFX : MonoBehaviour
{
    public PlayerFXType fxType;

    public void OnEnable() => SendPacket();

    private void SendPacket()
    {
        if (handlingPacket)
            return;

        var packet = new PlayerFXPacket(fxType, transform.position);
        Main.SendToAllOrServer(packet);
    }
}