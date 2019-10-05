using UnityEngine;

[RequireComponent(typeof(CameraFollow))]
public class CameraFollowPlayerShip : MonoBehaviour
{
    static CameraFollowPlayerShip Instance;

    static public void StopFollowingPlayer()
    {
        Instance.enabled = false;
    }
    static public void StartFollowingPlayer()
    {
        Instance.enabled = true;
    }

    private CameraFollow cameraFollowComponent;

    private void Awake()
    {
        cameraFollowComponent = GetComponent<CameraFollow>();
        PlayerInput.OnPlayerShipChanged += PlayerInput_OnPlayerShipChanged;
    }

    private void PlayerInput_OnPlayerShipChanged(GameObject obj)
    {
        if (enabled && obj != null)
        {
            cameraFollowComponent.SetFollowTarget(obj.transform);
        }
    }
}
