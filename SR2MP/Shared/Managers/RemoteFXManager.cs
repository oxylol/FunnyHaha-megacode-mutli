using Il2Cpp;
using SR2E.Utils;
using SR2MP.Components.FX;

namespace SR2MP.Shared.Managers
{
    public class RemoteFXManager
    {
        public readonly Dictionary<string, GameObject> allFX = new();
        public readonly Dictionary<string, SECTR_AudioCue> allCues = new();

        public Dictionary<PlayerFXType, GameObject> playerFXMap = new();
        public Dictionary<PlayerFXType, SECTR_AudioCue> playerAudioCueMap = new();

        public GameObject footstepFX;
        
        internal void Initialize()
        {
            allFX.Clear();

            // Cache all particles to avoid duplicate expensive FindObjectsOfTypeAll calls
            var allParticles = Resources.FindObjectsOfTypeAll<ParticleSystemRenderer>();

            foreach (var particle in allParticles)
            {
                var particleName = particle.gameObject.name.Replace(' ', '_');

                allFX.TryAdd(particleName, particle.gameObject);
            }
            allCues.Clear();
            foreach (var cue in Resources.FindObjectsOfTypeAll<SECTR_AudioCue>())
            {
                if (cue.Spatialization != SECTR_AudioCue.Spatializations.Simple2D)
                    cue.Spatialization = SECTR_AudioCue.Spatializations.Occludable3D;

                var cueName = cue.name.Replace(' ', '_');
                allCues.TryAdd(cueName, cue);
            }
            playerFXMap = new Dictionary<PlayerFXType, GameObject>
            {
                { PlayerFXType.None, null! },
                { PlayerFXType.VacReject, allFX["FX_vacReject"] },
                { PlayerFXType.VacAccept, allFX["FX_vacAcquire"] },
                { PlayerFXType.VacShoot, allFX["FX_VacpackShoot"] }
            };
            playerAudioCueMap = new Dictionary<PlayerFXType, SECTR_AudioCue>
            {
                { PlayerFXType.VacShootEmpty, allCues["VacShootEmpty"]},
                { PlayerFXType.VacHold, allCues["VacClogged"]},
                { PlayerFXType.VacSlotChange, allCues["VacAmmoSelect"]},
                { PlayerFXType.VacRunning, allCues["VacRun"]},
                { PlayerFXType.VacRunningStart, allCues["VacStart"]},
                { PlayerFXType.VacRunningEnd, allCues["VacEnd"]},
                { PlayerFXType.VacShootSound, allCues["VacShoot"]},
            };
            foreach (var playerFX in playerFXMap)
            {
                if (playerFX.Value)
                {
                    // Reuse cached particles instead of expensive second FindObjectsOfTypeAll call
                    foreach (var particle in allParticles)
                    {
                        if (particle.name.Contains(playerFX.Value.name))
                        {
                            if (!particle.GetComponent<NetworkPlayerFX>())
                                particle.AddComponent<NetworkPlayerFX>().fxType = playerFX.Key;
                        }
                    }
                }
            }
            
            footstepFX = allFX["FX_Footstep"];

            foreach (var cue in playerAudioCueMap)
            {
                cue.Value.Spatialization = SECTR_AudioCue.Spatializations.Occludable3D;
            }
            
            SrLogger.LogMessage("RemoteFXManager initialized", SrLogger.LogTarget.Both);
        }
        
        public bool TryGetFXType(SECTR_AudioCue cue, out PlayerFXType fxType)
        {
            var gotFx = false;
            fxType = PlayerFXType.None;
        
            foreach (var pair in playerAudioCueMap)
            {
                if (pair.Value == cue)
                {
                    fxType = pair.Key;
                    gotFx = true;
                
                    break;
                }
            }
        
            return gotFx;
        }
        
        public void PlayTransientAudio(SECTR_AudioCue cue, Vector3 position, bool loop = false)
        {
            SECTR_AudioSystem.Play(cue, position, loop);
        }
    }
}