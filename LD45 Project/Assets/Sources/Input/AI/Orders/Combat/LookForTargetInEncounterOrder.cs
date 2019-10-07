using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Similar to LookForTargetOrder but narrows down the search to the provided encounter.
/// </summary>
public class LookForTargetInEncounterOrder : AIState
{
    Encounter encounter;

    Action<EncounterAgent> onTargetFoundCallback;

    public LookForTargetInEncounterOrder(GameObject ship, Encounter encounter, Action<EncounterAgent> targetFoundCallback) : base(ship)
    {
        onTargetFoundCallback = targetFoundCallback;
        this.encounter = encounter;
    }

    public LookForTargetInEncounterOrder(GameObject ship, Action<EncounterAgent> targetFoundCallback) : base(ship)
    {
        onTargetFoundCallback = targetFoundCallback;
    }

    public void AssignEncounter(Encounter encounter)
    {
        this.encounter = encounter;
    }

    public override void Start()
    {
        List<EncounterAgent> potentialEnemies = encounter.GetEnemiesInEncounter(controlledShip.GetComponent<EncounterAgent>());

        float shortestSquaredDist = 0f;
        EncounterAgent closestEnemy = null;
        foreach (var enemy in potentialEnemies)
        {
                float squaredDist = (controlledShip.transform.position - enemy.transform.position).sqrMagnitude;
                Dockable dockableComponent = enemy.GetComponent<Dockable>();
                if (dockableComponent == null || dockableComponent.Docked == false)
                if ((closestEnemy == null || squaredDist < shortestSquaredDist))
                {
                    closestEnemy = enemy;
                    shortestSquaredDist = squaredDist;
                }
        }

        if (closestEnemy != null)
        {
            Succeed();
            onTargetFoundCallback(closestEnemy);
        }
        else
        {
            Fail("ENCOUNTER OVER");
        }
    }

    public override void Update()
    {

    }
}