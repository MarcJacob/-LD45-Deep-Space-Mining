using System;
using UnityEngine;

public class SeekMineableAsteroidOrder : AIState
{
    Action<Mineable> targetFoundCallback;

    public SeekMineableAsteroidOrder(GameObject ship, Action<Mineable> onTargetFoundCallback) : base(ship)
    {
        targetFoundCallback = onTargetFoundCallback;
    }

    public override void Start()
    {
        Mineable[] mineableObjects = GameObject.FindObjectsOfType<Mineable>();
        float closestDist = -1f;
        Mineable closestMineable = null;
        var beam = controlledShip.GetComponent<MiningBeam>();
        foreach (var obj in mineableObjects)
        {
            if (obj.ResourceType == beam.MinedResource)
            {
                float dist = (controlledShip.transform.position - obj.transform.position).sqrMagnitude;
                if (closestMineable == null || dist < closestDist)
                {
                    closestMineable = obj;
                    closestDist = dist;
                }
            }
        }

        if (closestMineable != null)
        {
            Succeed();
            targetFoundCallback(closestMineable);
        }
        else
        {
            Fail("NO TARGET FOUND");
        }
    }

    public override void Update()
    {
    }
}
