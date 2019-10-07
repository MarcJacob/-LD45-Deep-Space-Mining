using UnityEngine;

public class ResolveEncounterOrder : AIState
{
    const float LookForCloserTargetPeriod = 1f;

    private AttackTargetOrder attackTarget;
    private LookForTargetInEncounterOrder lookForTargetInEncounter;
    private EncounterAgent encounterAgent;

    private float currentLookForCloserTargetCooldown;

    public ResolveEncounterOrder(GameObject ship, EncounterAgent encounterAgent) : base(ship)
    {
        this.encounterAgent = encounterAgent;

        this.encounterAgent.OnEncounterLeft += EncounterAgent_OnEncounterLeft;
        this.encounterAgent.OnEncounterJoined += EncounterAgent_OnEncounterJoined;

        float combatRange = 10f;
        if (ship.GetComponent<ShipProperties>().Size == SHIP_SIZE.M) combatRange = 40f;

        attackTarget = new AttackTargetOrder(ship, combatRange);
        lookForTargetInEncounter = new LookForTargetInEncounterOrder(ship, OnTargetFound);
        lookForTargetInEncounter.OnStateFailed += LookForTargetInEncounter_OnStateFailed;
    }

    private void EncounterAgent_OnEncounterJoined(Encounter encounter)
    {
        lookForTargetInEncounter.AssignEncounter(encounter);
    }

    private void EncounterAgent_OnEncounterLeft(Encounter obj)
    {
        Succeed();
    }

    private void LookForTargetInEncounter_OnStateFailed(string reason)
    {
        Debug.LogWarning("Couldn't find target in encounter. Reason : " + reason);
    }

    private void OnTargetFound(EncounterAgent target)
    {
        if (attackTarget.Target != target)
        {
            attackTarget.AssignNewTarget(target);
        }
    }

    public override void Start()
    {
        controlledShip.GetComponent<Dockable>().Undock();
    }

    public override void Update()
    {
        currentLookForCloserTargetCooldown -= Time.deltaTime;
        if (currentLookForCloserTargetCooldown <= 0f)
        {
            lookForTargetInEncounter.Start();
        }
        attackTarget.Update();
    }

    public override AIInputData OnOrderInput()
    {
        return attackTarget.OnOrderInput();
    }
}
