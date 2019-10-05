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

    private ShipPiloting shipControllerInput;

    float[] mineBuffer;

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
                var mineableCargo = target.GetComponent<Cargo>();

                if (mineableCargo.HasResource((uint)mineableResource) && (transform.position - target.transform.position).sqrMagnitude < range * range)
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
                }
                else
                {
                    Debug.DrawLine(transform.position, shipControllerInput.CurrentPilot.targetedPosition + (Vector2)transform.position, Color.red);

                }
            }
            else
            {
                for (int i = 0; i < mineBuffer.Length; i++)
                    mineBuffer[i] = 0f;
            }
        }
    }
}