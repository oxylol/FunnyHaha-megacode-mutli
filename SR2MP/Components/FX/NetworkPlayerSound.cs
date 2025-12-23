using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace SR2MP.Components.FX;

[RegisterTypeInIl2Cpp(false)]
public sealed class NetworkPlayerSound : MonoBehaviour
{
    public PlayerFXType fxType;

    private bool cachedIsPlaying = false;
    private SECTR_AudioCue cachedAudioCue;
    private SECTR_PointSource audioSource;

    public bool IsPlaying => audioSource.IsPlaying && !audioSource.instance.Paused;
    public SECTR_AudioCue AudioCue => audioSource.Cue;

    public void Start()
    {
        audioSource = GetComponent<SECTR_PointSource>();
    }

    public void Update()
    {
        var hasChanged =  IsPlaying != cachedIsPlaying || AudioCue != cachedAudioCue;

        cachedIsPlaying = IsPlaying;
        cachedAudioCue = AudioCue;

        if (!hasChanged)
            return;

        SendPacket();
    }

    public void SendPacket()
    {
        // Defaults to PlayerFXType.None
        if (!fxManager.TryGetFXType(audioSource.Cue, out fxType))
        {
            return;
        }

        var packet = new PlayerFXPacket(fxType, LocalID);
        Main.SendToAllOrServer(packet);
    }
}