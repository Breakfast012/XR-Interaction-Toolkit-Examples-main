using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class ViewSwitcherSingleCamera : MonoBehaviour
{
    [Header("XR Origin & Controllers")]
    public GameObject xrOrigin;                      // 场景中唯一 XR Origin
    public GameObject leftController;
    public GameObject rightController;
    public ContinuousTurnProvider turnProvider;

    [Header("Input")]
    public InputActionProperty switchAction;         // 切换按钮，如 XRI RightHand Secondary

    // 保存 FP 起始点
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
            // 记录初始 FP 位置与朝向
            initialPosition = xrOrigin.transform.position;
            initialRotation = xrOrigin.transform.rotation;
            // 启动 GhostFly
            xrOrigin.GetComponent<GhostFlyControl2>().enabled = true;

            // 显式启用 InputAction
            xrOrigin.GetComponent<GhostFlyControl2>().moveAction.action.Enable();
            xrOrigin.GetComponent<GhostFlyControl2>().upDownAction.action.Enable();

            turnProvider.enabled = false;
            leftController.SetActive(false);
            rightController.SetActive(false);

            Debug.Log("[Switcher] Now in Ghost mode.");
        }
        else
        {
            // 复位位置 + 禁用 GhostFly
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
使用说明：
1. 在 XR Origin 根节点上挂载 GhostFlyControl 和 ViewSwitcherSingleCamera。
2. 将 GhostFlyControl 默认禁用，Inspector 绑定 moveAction, upDownAction, headReference。
3. 在 ViewSwitcherSingleCamera Inspector 中：
   - xrOrigin 拖入 XR Origin
   - moveProvider/turnProvider 绑定 XR Origin 上的对应组件
   - leftInteractor/rightInteractor 绑定 XR Origin 下手柄的 XRRayInteractor
   - switchAction 绑定右手 Secondary Button
4. Play 时：
   - 默认 FP 模式，GhostFlyControl disabled
   - 按下切换键：进入 Ghost 模式，冻结交互，启用 GhostFlyControl 即可飞行
   - 再次按键：恢复 FP 模式，回到初始位置，禁用 GhostFlyControl
*/
