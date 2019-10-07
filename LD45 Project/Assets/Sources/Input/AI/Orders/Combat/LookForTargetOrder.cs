using System;
using System.Collections.Generic;
using UnityEngine;

public class LookForTargetOrder : AIState
{
    Action<EncounterAgent> targetFoundCallback;
    float maxDistance = 0f;

    public LookForTargetOrder(GameObject ship, Action<EncounterAgent> targetFoundCallback, float maxDistance) : base(ship)
    {
        this.targetFoundCallback = targetFoundCallback;
        this.maxDistance = maxDistance;
    }

    public override void Start()
    {
        uint myFaction = controlledShip.GetComponent<Ownership>().OwnerFactionID;

        List<Ownership[]> potentialEnemies = new List<Ownership[]>();
        for(int facID = 1; facID < Ownership.FactionCount; facID++)
        {
            if (facID != myFaction)
            {
                potentialEnemies.Add(Ownership.GetFactionMembers(facID));
            }
        }

        float shortestSquaredDist = 0f;
        EncounterAgent closestEnemy = null;
        foreach(var enemyGroup in potentialEnemies)
        {
            foreach(var enemy in enemyGroup)
            {
                EncounterAgent encounterAgent = enemy.GetComponent<EncounterAgent>();
                if (encounterAgent != null)
                {
                    float squaredDist = (controlledShip.transform.position - enemy.transform.position).sqrMagnitude;
                    if (squaredDist < maxDistance * maxDistance && (closestEnemy == null || squaredDist < shortestSquaredDist))
                    {
                        closestEnemy = encounterAgent;
                        shortestSquaredDist = squaredDist;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            Succeed();
            targetFoundCallback(closestEnemy);
        }
        else
        {
            Fail("NO ENEMY IN RANGE");
        }
    }

    public override void Update()
    {
    }
}
