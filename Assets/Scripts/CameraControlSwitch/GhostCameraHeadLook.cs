using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCameraHeadLook : MonoBehaviour
{
    public Transform headReference;     // XR Camera
    public Transform ghostCamera;       // GhostCamera ∂‘œÛ

    void Update()
    {
        ghostCamera.rotation = headReference.rotation;
    }
}
