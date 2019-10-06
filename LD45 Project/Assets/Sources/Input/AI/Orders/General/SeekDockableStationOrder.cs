using System;
using UnityEngine;

public class SeekDockableStationOrder : AIState
{
    Action<Dock> targetFoundCallback;

    public SeekDockableStationOrder(GameObject ship, Action<Dock> onTargetFoundCallback) : base(ship)
    {
        targetFoundCallback = onTargetFoundCallback;
    }

    public override void Start()
    {
        Dock[] mineableObjects = GameObject.FindObjectsOfType<Dock>();
        float closestDist = -1f;
        Dock closestDockable = null;

        foreach (var obj in mineableObjects)
        {
            float dist = (controlledShip.transform.position - obj.transform.position).sqrMagnitude;
            if (closestDockable == null || dist < closestDist)
            {
                closestDockable = obj;
                closestDist = dist;
            }
        }

        if (closestDockable != null)
        {
            Succeed();
            targetFoundCallback(closestDockable);
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
