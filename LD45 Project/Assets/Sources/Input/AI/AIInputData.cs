using UnityEngine;

public struct AIInputData
{
    public float acceleration;
    public float turn;
    public float strafe;
    public bool interacting;
    public bool usingEquipment;
    public UnityEngine.Vector2 targetPosition;
    public GameObject targetObject;
}
