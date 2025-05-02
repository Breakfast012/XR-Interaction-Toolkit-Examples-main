using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GhostFlyControl : MonoBehaviour
{
    public InputActionProperty moveAction;      // XRI RightHand/Move
    public InputActionProperty upDownAction;    // 例如 Trigger 值
    public float moveSpeed = 1.5f;
    public float verticalSpeed = 1f;
    public Transform headReference;             // XR Camera 用于方向参考

    void OnEnable()
    {
        moveAction.action.Enable();
        upDownAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        upDownAction.action.Disable();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        float upDownInput = upDownAction.action.ReadValue<float>();

        if (headReference == null) return;

        // 方向来自头部旋转（但我们不旋转自己）
        Vector3 forward = headReference.forward;
        Vector3 right = headReference.right;

        Vector3 move = forward * moveInput.y + right * moveInput.x + Vector3.up * upDownInput;

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}

