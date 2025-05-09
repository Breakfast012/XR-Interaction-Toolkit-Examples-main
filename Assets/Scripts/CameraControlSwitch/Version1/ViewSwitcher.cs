using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class ViewSwitcher : MonoBehaviour
{
    [Header("References")]
    public GameObject ghostRig;                // Ghost 模式的飞行器
    public Camera ghostCamera;                 // Ghost 模式的观察相机
    public GameObject xrOrigin;                // XR Origin 本体，用于角色控制
    public Camera xrCamera;                    // XR 模式主相机（Main Camera）

    [Header("Providers")]
    public ContinuousMoveProvider moveProvider; // XR Origin 上的移动组件
    public ContinuousTurnProvider turnProvider; // XR Origin 上的转向组件

    [Header("Input")]
    public InputActionProperty switchAction;    // 视角切换按钮（如 XRI RightHand Secondary）

    private bool isGhost = false;

    void OnEnable()
    {
        switchAction.action.Enable();
    }

    void OnDisable()
    {
        switchAction.action.Disable();
    }

    void Update()
    {
        if (switchAction.action.WasPressedThisFrame())
        {
            isGhost = !isGhost;

            // 切换 GhostRig 和相机渲染
            ghostRig.SetActive(isGhost);
            ghostCamera.enabled = isGhost;
            xrCamera.enabled = !isGhost;

            // 只启用/禁用角色移动和旋转
            moveProvider.enabled = !isGhost;
            turnProvider.enabled = !isGhost;
        }
    }
}
