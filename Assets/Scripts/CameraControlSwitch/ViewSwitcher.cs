using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class ViewSwitcherSingleCamera : MonoBehaviour
{
    [Header("XR Origin & Controllers")]
    public GameObject xrOrigin;                      // ������Ψһ XR Origin
    public GameObject leftController;
    public GameObject rightController;
    public ContinuousTurnProvider turnProvider;

    [Header("Input")]
    public InputActionProperty switchAction;         // �л���ť���� XRI RightHand Secondary

    // ���� FP ��ʼ��
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isGhost = false;

    void Awake()
    {
        
    }

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
        if (!switchAction.action.WasPressedThisFrame())
            return;

        isGhost = !isGhost;

        Debug.Log($"[Switcher] Switching to {(isGhost ? "Ghost" : "FirstPerson")} Mode");

        if (isGhost)
        {
            // ��¼��ʼ FP λ���볯��
            initialPosition = xrOrigin.transform.position;
            initialRotation = xrOrigin.transform.rotation;
            // ���� GhostFly
            xrOrigin.GetComponent<GhostFlyControl2>().enabled = true;

            // ��ʽ���� InputAction
            xrOrigin.GetComponent<GhostFlyControl2>().moveAction.action.Enable();
            xrOrigin.GetComponent<GhostFlyControl2>().upDownAction.action.Enable();

            turnProvider.enabled = false;
            leftController.SetActive(false);
            rightController.SetActive(false);

            Debug.Log("[Switcher] Now in Ghost mode.");
        }
        else
        {
            // ��λλ�� + ���� GhostFly
            xrOrigin.GetComponent<GhostFlyControl2>().enabled = false;
            xrOrigin.GetComponent<GhostFlyControl2>().moveAction.action.Disable();
            xrOrigin.GetComponent<GhostFlyControl2>().upDownAction.action.Disable();

            xrOrigin.transform.position = initialPosition;
            xrOrigin.transform.rotation = initialRotation;

            turnProvider.enabled = true;
            leftController.SetActive(true);
            rightController.SetActive(true);

            Debug.Log("[Switcher] Now in FP mode.");
        }
    }
}

/*
ʹ��˵����
1. �� XR Origin ���ڵ��Ϲ��� GhostFlyControl �� ViewSwitcherSingleCamera��
2. �� GhostFlyControl Ĭ�Ͻ��ã�Inspector �� moveAction, upDownAction, headReference��
3. �� ViewSwitcherSingleCamera Inspector �У�
   - xrOrigin ���� XR Origin
   - moveProvider/turnProvider �� XR Origin �ϵĶ�Ӧ���
   - leftInteractor/rightInteractor �� XR Origin ���ֱ��� XRRayInteractor
   - switchAction ������ Secondary Button
4. Play ʱ��
   - Ĭ�� FP ģʽ��GhostFlyControl disabled
   - �����л��������� Ghost ģʽ�����ύ�������� GhostFlyControl ���ɷ���
   - �ٴΰ������ָ� FP ģʽ���ص���ʼλ�ã����� GhostFlyControl
*/
