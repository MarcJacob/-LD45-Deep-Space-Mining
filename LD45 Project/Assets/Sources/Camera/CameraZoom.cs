using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private float maxZoom = -4f;
    [SerializeField]
    private float maxDezoom = -30f;
    [SerializeField]
    private float zoomDezoomSpeed = 1f;
    [SerializeField]
    private float zoomLerpSpeed = 1f;


    float desiredZoomLevel;
    private void Awake()
    {
        desiredZoomLevel = transform.position.z;
    }
    private void Update()
    {
        Vector2 scroll = Input.mouseScrollDelta;
        if (scroll.y > 0f && desiredZoomLevel < transform.position.z) desiredZoomLevel = transform.position.z;
        else if (scroll.y < 0f && desiredZoomLevel > transform.position.z) desiredZoomLevel = transform.position.z;

        desiredZoomLevel += scroll.y * zoomDezoomSpeed;
        desiredZoomLevel = Mathf.Clamp(desiredZoomLevel, maxDezoom, maxZoom);

        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(Mathf.Lerp(pos.z, desiredZoomLevel, Time.deltaTime * zoomLerpSpeed), maxDezoom, maxZoom);
        transform.position = pos;
    }
}