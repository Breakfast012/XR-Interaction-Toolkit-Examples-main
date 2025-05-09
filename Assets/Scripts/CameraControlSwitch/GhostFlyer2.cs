using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GhostFlyControl2 : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionProperty moveAction;    // e.g. XRI RightHand/Move
    public InputActionProperty upDownAction;  // e.g. XRI RightHand/Activate (扳机)

    [Header("Flight Settings")]
    public float moveSpeed = 1.5f;
    public float verticalSpeed = 1f;
    public Transform headReference;           // XR Origin 下的 Camera

    void OnEnable()
    {
        Debug.Log("[GhostFly] Enabled - ready to fly.");
        moveAction.action.Enable();
        upDownAction.action.Enable();
    }

    void OnDisable()
    {
        Debug.Log("[GhostFly] Disabled.");
        moveAction.action.Disable();
        upDownAction.action.Disable();
    }

    void Update()
    {
        if (headReference == null)
        {
            Debug.LogWarning("[GhostFly] Missing head reference.");
            return;
        }
        if (!moveAction.action.enabled)
            Debug.LogWarning("[GhostFly] moveAction is not enabled.");
        if (!upDownAction.action.enabled)
            Debug.LogWarning("[GhostFly] upDownAction is not enabled.");

        Vector2 leftInput = moveAction.action.ReadValue<Vector2>();
        Vector2 rightInput = upDownAction.action.ReadValue<Vector2>();


        Vector3 forward = headReference.forward;
        Vector3 right = headReference.right;
        Vector3 up = Vector3.up;

        // 左手控制：水平位移
        Vector3 move = right * leftInput.x + new Vector3(forward.x, 0, forward.z).normalized * leftInput.y;
        // 右手控制：自由飞行（包括上下）
        move += Vector3.up * rightInput.y;

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}