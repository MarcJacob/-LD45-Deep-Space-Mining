using UnityEngine;
/// <summary>
/// "Sub order queue". Doesn't really have a code of its own - simply runs a sequence of two orders.
/// SeekDockableStation -> Dock.
/// </summary>
public class DockAtNearestOrder : AIState
{
    SeekDockableStationOrder seekOrder;
    DockAtOrder dockAtTargetOrder;

    bool targetFound = false;

    public DockAtNearestOrder(GameObject ship) : base(ship)
    {
        seekOrder = new SeekDockableStationOrder(ship, OnDockableTargetFound);
        dockAtTargetOrder = new DockAtOrder(ship);

        seekOrder.OnStateFailed += OnSubOrderFailed;
        dockAtTargetOrder.OnStateFailed += OnSubOrderFailed;
        dockAtTargetOrder.OnStateSucceeded += OnAsteroidMined;
    }

    private void OnAsteroidMined()
    {
        Debug.Log("AsteroidMined()");
        Succeed();
        targetFound = false;
    }

    void OnDockableTargetFound(Dock target)
    {
        dockAtTargetOrder.AssignTarget(target);
        dockAtTargetOrder.Start();
        targetFound = true;
        Debug.Log("Target found");
    }

    void OnSubOrderFailed(string reason)
    {
        Fail(reason);
        targetFound = false;
    }

    public override void Start()
    {
        seekOrder.Start();
    }

    public override void Update()
    {
        if (targetFound)
        {
            dockAtTargetOrder.Update();
        }
    }

    public override AIInputData OnOrderInput()
    {
        if (!targetFound)
        {
            return base.OnOrderInput();
        }
        else
        {
            return dockAtTargetOrder.OnOrderInput();
        }
    }
}
