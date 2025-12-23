using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace SR2MP.Components;

[RegisterTypeInIl2Cpp(false)]
public sealed class TransformLookAtCamera : MonoBehaviour
{
    public Transform targetTransform;

    private bool isText;

    public void Start() => isText = targetTransform.GetComponent<TextMeshPro>();

    public void Update()
    {
        targetTransform.LookAt(Camera.main.transform);

        if (isText)
            targetTransform.Rotate(0, 180, 0);
    }
}