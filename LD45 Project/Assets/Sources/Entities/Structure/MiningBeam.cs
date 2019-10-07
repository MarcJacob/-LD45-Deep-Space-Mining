using System;
using UnityEngine;

public class MiningBeam : MonoBehaviour
{
    [SerializeField]
    private float miningStrength;
    [SerializeField]
    private float range;
    [SerializeField]
    private RESOURCE_TYPE mineableResource;
    [SerializeField]
    private Cargo cargo;

    public event Action<Vector2, GameObject> OnActivation = delegate { };
    public event Action OnDeactivation = delegate { };
    public event Action<Vector2, GameObject> OnRunning = delegate { }; // Called each frame while the beam is active.

    private ShipPiloting shipControllerInput;

    float[] mineBuffer;

    public float Range
    {
        get
        {
            return range;
        }
    }

    public RESOURCE_TYPE MinedResource { get { return mineableResource; } }

    private void Awake()
    {
        shipControllerInput = GetComponent<ShipPiloting>();
        mineBuffer = new float[Enum.GetNames(typeof(RESOURCE_TYPE)).Length];
    }

    private void Update()
    {
        if (shipControllerInput.CurrentPilot != null)
        {
            GameObject target = shipControllerInput.CurrentPilot.targetedObject;
            if (shipControllerInput.CurrentPilot.usingActiveEquipment && target != null && target.GetComponent<Mineable>())
            {
                var mineable = target.GetComponent<Mineable>();

                if (mineable.ResourceType == mineableResource && (transform.position - target.transform.position).sqrMagnitude < range * range)
                {
                    ActivateMiningOn(mineable);
                }
                else
                {
                    Debug.DrawLine(transform.position, shipControllerInput.CurrentPilot.targetedPosition + (Vector2)transform.position, Color.red);
                    DeactivateMining();
                }
            }
            else
            {
                DeactivateMining();
            }
        }
        else if (activatedLastFrame)
        {
            DeactivateMining();
        }
    }
    private bool activatedLastFrame = false;
    private void DeactivateMining()
    {
        for (int i = 0; i < mineBuffer.Length; i++)
            mineBuffer[i] = 0f;
        if (activatedLastFrame)
        {
            activatedLastFrame = false;
            OnDeactivation();
        }
    }

    private void ActivateMiningOn(Mineable mineable)
    {
        Debug.DrawLine(transform.position, shipControllerInput.CurrentPilot.targetedPosition + (Vector2)transform.position);
        mineBuffer[(uint)mineableResource] += miningStrength * Time.deltaTime;
        if (mineBuffer[(uint)mineableResource] >= 1f)
        {
            uint minedInBuffer = (uint)Mathf.FloorToInt(mineBuffer[(uint)mineableResource]);
            mineBuffer[(uint)mineableResource] -= minedInBuffer;

            uint mined = mineable.Mine((uint)mineableResource, minedInBuffer);
            if (!cargo.AddResource((uint)mineableResource, mined))
            {
                Debug.LogWarning("Warning - Not enough cargo to fit mined resources ! Yield has been lost.");
            }
        }
        if (!activatedLastFrame)
        {
            activatedLastFrame = true;
            OnActivation(shipControllerInput.CurrentPilot.targetedPosition + (Vector2)transform.position, mineable.gameObject);
        }
        else
        {
            OnRunning(shipControllerInput.CurrentPilot.targetedPosition + (Vector2)transform.position, mineable.gameObject);
        }
    }
}