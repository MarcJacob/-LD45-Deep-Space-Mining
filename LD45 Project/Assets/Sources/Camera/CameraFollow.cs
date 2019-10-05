using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followed;
    [SerializeField]
    private float speedRatio;

    private void FixedUpdate()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, followed.position, Time.fixedDeltaTime * speedRatio);
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }

    public void SetFollowTarget(Transform target)
    {
        followed = target; // TODO : if the target is too far away, don't try to go there with unity units, "warp" the camera.
    }
}
