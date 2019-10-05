using UnityEngine;

public class EntityInput : MonoBehaviour
{
    public float acceleration { get; protected set; } // -1 = decellerating. 0 = neutral. 1 = accelerating.
    public float turningDirection { get; protected set; } // -1 = turning left. 0 = not turning. 1 = turning right.
    public float strafingDirection { get; protected set; } // -1 = strafing left. 0 = not strafing. 1 = strafing right.

    public bool usingActiveEquipment { get; protected set; }
    public Vector2 targetedPosition { get; protected set; }
    public GameObject targetedObject { get; protected set; }
    
    public bool interacting { get; protected set; }
}
