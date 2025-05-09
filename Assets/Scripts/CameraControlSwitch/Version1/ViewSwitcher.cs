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
    public GameObject ghostRig;                // Ghost ģʽ�ķ�����
    public Camera ghostCamera;                 // Ghost ģʽ�Ĺ۲����
    public GameObject xrOrigin;                // XR Origin ���壬���ڽ�ɫ����
    public Camera xrCamera;                    // XR ģʽ�������Main Camera��

    [Header("Providers")]
    public ContinuousMoveProvider moveProvider; // XR Origin �ϵ��ƶ����
    public ContinuousTurnProvider turnProvider; // XR Origin �ϵ�ת�����

    [Header("Input")]
    public InputActionProperty switchAction;    // �ӽ��л���ť���� XRI RightHand Secondary��

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

            // �л� GhostRig �������Ⱦ
            ghostRig.SetActive(isGhost);
            ghostCamera.enabled = isGhost;
            xrCamera.enabled = !isGhost;

            // ֻ����/���ý�ɫ�ƶ�����ת
            moveProvider.enabled = !isGhost;
            turnProvider.enabled = !isGhost;
        }
    }
}
