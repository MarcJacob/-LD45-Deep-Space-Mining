using UnityEngine;

public class DefendAreaBehavior : AIBehavior
{
    const float LookForTargetDelay = 2f; // If the AI just looked for a target and didn't find anything, a delay is applied to save compute time.

    private Vector2 areaCenter;
    private float radius;
    private bool inEncounter = false;
    private float squaredDistToArea = 0f;

    private GoToOrder goToAreaCenterOrder;
    private LookForTargetOrder findTargetOrder;
    private ResolveEncounterOrder resolveEncounterOrder;
    private bool goingToArea = false;

    private EncounterAgent controlledEncounterAgent;

    private float lookForTargetDelay = 0f;

    public DefendAreaBehavior(Vector2 area, float areaRadius, GameObject ship, AIOrderQueue orderQueue) : base(ship, orderQueue)
    {
        areaCenter = area;
        radius = areaRadius;
        goToAreaCenterOrder = new GoToOrder(ship, area, areaRadius);
        goToAreaCenterOrder.OnStateSucceeded += GoToAreaCenterOrder_OnStateSucceeded;

        controlledEncounterAgent = controlledShip.GetComponent<EncounterAgent>();
        controlledEncounterAgent.OnEncounterJoined += ControlledEncounterAgent_OnEncounterJoined;
        controlledEncounterAgent.OnEncounterLeft += ControlledEncounterAgent_OnEncounterLeft;
        resolveEncounterOrder = new ResolveEncounterOrder(ship, controlledEncounterAgent);

        findTargetOrder = new LookForTargetOrder(ship, OnTargetFound, areaRadius);
    }

    private void GoToAreaCenterOrder_OnStateSucceeded()
    {
        goingToArea = false;
    }

    public override void StartBehavior()
    {

        UpdateDistToArea();
    }

    private void ControlledEncounterAgent_OnEncounterLeft(Encounter encounter)
    {
        inEncounter = false;
    }

    private void ControlledEncounterAgent_OnEncounterJoined(Encounter encounter)
    {
        inEncounter = true;
        orderQueue.AssignOrder(resolveEncounterOrder);
    }

    private void UpdateDistToArea()
    {
        squaredDistToArea = (areaCenter - (Vector2)controlledShip.transform.position).sqrMagnitude;
    }

    public override void StopBehavior()
    {

    }

    public override void UpdateBehavior()
    {
        UpdateDistToArea();
        if (!goingToArea && !inEncounter && squaredDistToArea > radius * radius)
        {
            // We are away from the area and not in a fight. Let's go to the area.
            orderQueue.AssignOrder(goToAreaCenterOrder);
            goingToArea = true;
        }
        else if (!goingToArea && !inEncounter)
        {
            // We are in the area, but not in a fight. Let's look for the nearest target.
            lookForTargetDelay -= Time.deltaTime;
            if (lookForTargetDelay < 0f)
            {
                orderQueue.AssignOrder(findTargetOrder);
            }
        }
        else if (!goingToArea)
        {
            // We are in an encounter. The order to resolve it has probably already been assigned.
            // TODO Check for distance to area and maybe try to leave encounter to come back to the area (prevents chasing too far away).
        }
    }

    private void OnTargetFound(EncounterAgent target)
    {
        // When a target is found, we start an Encounter.
        // if the target is already in an encounter, then we join it ?

        if (target.CurrentEncounter == null)
        {
            controlledEncounterAgent.StartEncounter(target);
        }
        else
        {
            target.CurrentEncounter.AddToEncounter(controlledEncounterAgent);
        }
    }
}
