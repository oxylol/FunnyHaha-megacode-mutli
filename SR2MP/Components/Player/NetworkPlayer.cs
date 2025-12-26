using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppMonomiPark.SlimeRancher.Player.PlayerItems;
using Il2CppTMPro;
using MelonLoader;
using SR2E.Utils;
using SR2MP.Client.Models;
using SR2MP.Components.FX;
using SR2MP.Components.Utils;
using static SR2E.ContextShortcuts;
using static SR2MP.Shared.Utils.Timers;

namespace SR2MP.Components.Player
{
    [RegisterTypeInIl2Cpp(false)]
    public partial class NetworkPlayer : MonoBehaviour
    {
        private MeshRenderer[] renderers;
        private Collider collider;

        internal Vector3 previousPosition;
        internal Vector3 nextPosition;

        internal Vector2 previousRotation;
        internal Vector2 nextRotation;

        private float interpolationStart;
        private float interpolationEnd;

        public TextMeshPro usernamePanel;

        private float transformTimer = PlayerTimer;
        private int meshReloadFrameCounter = 0;
        private const int MeshReloadInterval = 10; // Reload every 10 frames instead of every frame

        private Animator animator;
        private bool hasAnimationController = false;

        internal RemotePlayer model;
        
        internal Transform camera;

        public string ID { get; internal set; }

        public bool IsLocal { get; internal set; } = false;

        // Cache font to avoid expensive Resources.FindObjectsOfTypeAll() on every SetUsername call
        private static TMP_FontAsset? _cachedUsernameFont;
        private static TMP_FontAsset GetCachedFont(string fontName)
        {
            if (_cachedUsernameFont == null)
            {
                _cachedUsernameFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(x => x.name == fontName)!;
            }
            return _cachedUsernameFont;
        }

        public void SetUsername(string username)
        {
            usernamePanel = transform.GetChild(1).GetComponent<TextMeshPro>();
            usernamePanel.text = username;
            usernamePanel.alignment = TextAlignmentOptions.Center;
            usernamePanel.fontSize = 3;
            usernamePanel.font = GetCachedFont("Runsell Type - HemispheresCaps2 (Latin)");
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
            if (IsLocal)
            {
                camera = GetComponent<SRCharacterController>()._cameraController.transform;
                GetComponent<PlayerItemController>()._vacuumItem.AddComponent<NetworkPlayerSound>();
            }
            
            usernamePanel = transform.GetChild(1).GetComponent<TextMeshPro>();

            if (usernamePanel)
            {
                usernamePanel.gameObject.AddComponent<TransformLookAtCamera>().targetTransform =
                    usernamePanel.transform;

                SetUsername(gameObject.name);
            }

            SetupRenderersAndCollision();
        }

        void SetupRenderersAndCollision()
        {
            if (IsLocal)
            {
                var modelRenderers = GetComponentsInChildren<MeshRenderer>();
                var cameraRenderers = camera.GetComponentsInChildren<MeshRenderer>();
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
                model = playerManager.GetPlayer(ID) ?? playerManager.AddPlayer(ID);
                return;
            }

            transformTimer -= UnityEngine.Time.unscaledDeltaTime;
            if (!IsLocal)
            {
                float timer = Mathf.InverseLerp(interpolationStart, interpolationEnd, UnityEngine.Time.unscaledTime);
                timer = Mathf.Clamp01(timer);

                transform.position = Vector3.Lerp(previousPosition, nextPosition, timer);

                receivedLookY = Mathf.LerpAngle(previousRotation.y, nextRotation.y, timer);
                transform.eulerAngles = new Vector3(0,  Mathf.LerpAngle(previousRotation.x, nextRotation.x, timer), 0);
            }

            // Throttle mesh reload to reduce performance overhead
            meshReloadFrameCounter++;
            if (meshReloadFrameCounter >= MeshReloadInterval)
            {
                meshReloadFrameCounter = 0;
                ReloadMeshTransform();
            }
            if (transformTimer < 0)
            {
                transformTimer = PlayerTimer;

                if (IsLocal)
                {
                    playerManager.SendPlayerUpdate(
                        position: transform.position,
                        rotation: transform.eulerAngles.y,
                        horizontalMovement: animator.GetFloat("HorizontalMovement"),
                        forwardMovement: animator.GetFloat("ForwardMovement"),
                        yaw: animator.GetFloat("Yaw"),
                        airborneState: animator.GetInteger("AirborneState"),
                        moving: animator.GetBool("Moving"),
                        horizontalSpeed: animator.GetFloat("HorizontalSpeed"),
                        forwardSpeed: animator.GetFloat("ForwardSpeed"),
                        sprinting: animator.GetBool("Sprinting"),
                        lookY: camera.eulerAngles.x
                    );
                }
                else
                {
                    if (!hasAnimationController)
                    {
                        var playerAnimatorController = sceneContext.player?.GetComponent<Animator>().runtimeAnimatorController;
                        
                        if (animator.runtimeAnimatorController != null)
                        {
                            hasAnimationController = true;
                            animator.runtimeAnimatorController =
                                Object.Instantiate(playerAnimatorController);
                            animator.avatar = sceneContext.player?.GetComponent<Animator>().avatar;
                            SetupAnimations();
                        }
                    }

                    nextPosition = model.Position;
                    previousPosition = transform.position;
                    nextRotation = new Vector2(model.Rotation, model.LookY);
                    previousRotation = new Vector2(transform.eulerAngles.y, model.LastLookY);

                    interpolationStart = UnityEngine.Time.unscaledTime;
                    interpolationEnd = UnityEngine.Time.unscaledTime + PlayerTimer;

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
            // Removed unnecessary bounds access - already throttled to every 10 frames
            if (!IsLocal)
            {
                // Toggle collider to refresh physics (throttled to reduce overhead)
                collider.enabled = false;
                collider.enabled = true;
            }
        }

        void LateUpdate()
        {
            AnimateArmY();
        }
    }
}