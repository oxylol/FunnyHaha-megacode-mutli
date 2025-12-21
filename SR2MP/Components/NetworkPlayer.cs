using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppTMPro;
using MelonLoader;
using SR2MP.Client.Managers;
using SR2MP.Client.Models;
using UnityEngine;
using UnityEngine.Animations;
using static SR2E.ContextShortcuts;
using static SR2MP.Shared.Utils.Timers;

namespace SR2MP.Components
{
    // todo: Fix the detachment issue qwq 3:
    [RegisterTypeInIl2Cpp(false)]
    public class NetworkPlayer : MonoBehaviour
    {
        private MeshRenderer[] renderers;
        private Collider collider;

        internal Vector3 previousPosition;
        internal Vector3 nextPosition;

        internal Quaternion previousRotation;
        internal Quaternion nextRotation;

        private float interpolationStart;
        private float interpolationEnd;

        public TextMeshPro usernamePanel;

        private float transformTimer = PlayerTimer;
        private float interpolationTimer = 0;

        private Animator animator;
        private bool hasAnimationController = false;

        internal RemotePlayer model;

        public string ID { get; internal set; }

        public bool IsLocal { get; internal set; } = false;

        private TMP_FontAsset GetFont(string fontName) => Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(x => x.name == fontName);
        public void SetUsername(string username)
        {
            usernamePanel = transform.GetChild(1).GetComponent<TextMeshPro>();
            usernamePanel.text = username;
            usernamePanel.alignment = TextAlignmentOptions.Center;
            usernamePanel.fontSize = 3;
            usernamePanel.font = GetFont("Runsell Type - HemispheresCaps2 (Latin)");
            if (!usernamePanel.GetComponent<TransformLookAtCamera>())
            {
                usernamePanel.gameObject.AddComponent<TransformLookAtCamera>().targetTransform =
                    usernamePanel.transform;
            }
        }

        void Awake()
        {
            if (transform.GetComponents<NetworkPlayer>().Length > 1)
            {
                Destroy(this);
                return;
            }

            animator = GetComponentInChildren<Animator>();

            if (animator == null)
            {
                SrLogger.LogWarning("NetworkPlayer has no Animator component!");
            }
        }

        void Start()
        {
            usernamePanel = transform.GetChild(1).GetComponent<TextMeshPro>();

            if (usernamePanel)
            {
                usernamePanel.gameObject.AddComponent<TransformLookAtCamera>().targetTransform =
                    usernamePanel.transform;

                SetUsername(gameObject.name);
            }

            if (IsLocal)
            {
                var modelRenderers = GetComponentsInChildren<MeshRenderer>();
                var cameraRenderers = GetComponent<SRCharacterController>()._cameraController.GetComponentsInChildren<MeshRenderer>();
                var allRenderers = new MeshRenderer[modelRenderers.Length + cameraRenderers.Length];
                
                modelRenderers.CopyTo(allRenderers, 0);
                cameraRenderers.CopyTo(allRenderers, modelRenderers.Length);
                
                renderers = allRenderers;
            }
            else { renderers = GetComponentsInChildren<MeshRenderer>(); }

            collider = GetComponentInChildren<Collider>();
        }

        public void Update()
        {
            if (model == null)
            {
                model = playerManager.GetPlayer(ID);
                // If there was never a model to begin with
                if (model == null)
                {
                    model = playerManager.AddPlayer(ID);
                }

                return;
            }

            transformTimer -= Time.unscaledDeltaTime;
            if (!IsLocal)
            {
                float timer = Mathf.InverseLerp(interpolationStart, interpolationEnd, Time.unscaledTime);
                timer = Mathf.Clamp01(timer);

                transform.position = Vector3.Lerp(previousPosition, nextPosition, timer);
                transform.rotation = Quaternion.Lerp(previousRotation, nextRotation, timer);
                // Not sure if its necessary
                transform.hasChanged = true;
            }

            ReloadMeshTransform();
            if (transformTimer < 0)
            {
                transformTimer = PlayerTimer;

                if (IsLocal)
                {
                    playerManager.SendPlayerUpdate(
                        position: transform.position,
                        rotation: transform.rotation,
                        horizontalMovement: animator.GetFloat("HorizontalMovement"),
                        forwardMovement: animator.GetFloat("ForwardMovement"),
                        yaw: animator.GetFloat("Yaw"),
                        airborneState: animator.GetInteger("AirborneState"),
                        moving: animator.GetBool("Moving"),
                        horizontalSpeed: animator.GetFloat("HorizontalSpeed"),
                        forwardSpeed: animator.GetFloat("ForwardSpeed"),
                        sprinting: animator.GetBool("Sprinting")
                    );
                }
                else
                {
                    if (!hasAnimationController)
                    {
                        animator.runtimeAnimatorController =
                            sceneContext.player?.GetComponent<Animator>().runtimeAnimatorController;
                        animator.avatar = sceneContext.player?.GetComponent<Animator>().avatar;

                        if (animator.runtimeAnimatorController != null)
                            hasAnimationController = true;
                    }

                    nextPosition = model.Position;
                    previousPosition = transform.position;
                    nextRotation = model.Rotation;
                    previousRotation = transform.rotation;

                    interpolationStart = Time.unscaledTime;
                    interpolationEnd = Time.unscaledTime + PlayerTimer;

                    animator.SetFloat("HorizontalMovement", model.HorizontalMovement);
                    animator.SetFloat("ForwardMovement", model.ForwardMovement);
                    animator.SetFloat("Yaw", model.Yaw);
                    animator.SetInteger("AirborneState", model.AirborneState);
                    animator.SetBool("Moving", model.Moving);
                    animator.SetFloat("HorizontalSpeed", model.HorizontalSpeed);
                    animator.SetFloat("ForwardSpeed", model.ForwardSpeed);
                    animator.SetBool("Sprinting", model.Sprinting);
                }
            }
        }

        void ReloadMeshTransform()
        {
            foreach (var renderer in renderers)
            {
                // This is for the getter to refresh the render position stuff qwq
                var bounds = renderer.bounds;
                var localBounds = renderer.localBounds;
            }

            if (!IsLocal)
            {
                // This is for the 
                collider.enabled = false;
                collider.enabled = true;
            }
        }
    }
}