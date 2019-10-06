using UnityEngine;
/// <summary>
/// "Sub order queue". Doesn't really have a code of its own - simply runs a sequence of two orders.
/// SeekMineableAsteroid -> Mine.
/// </summary>
public class MineNearest : AIState
{
    SeekMineableAsteroidOrder seekOrder;
    MineTarget mineTargetOrder;

    bool targetFound = false;

    public MineNearest(GameObject ship) : base(ship)
    {
        seekOrder = new SeekMineableAsteroidOrder(ship, OnMineableTargetFound);
        mineTargetOrder = new MineTarget(ship);

        seekOrder.OnStateFailed += OnSubOrderFailed;
        mineTargetOrder.OnStateFailed += OnSubOrderFailed;
        mineTargetOrder.OnStateSucceeded += OnAsteroidMined;
    }

    private void OnAsteroidMined()
    {
        Debug.Log("AsteroidMined()");
        Succeed();
        targetFound = false;
    }

    void OnMineableTargetFound(Mineable target)
    {
        mineTargetOrder.AssignTarget(target);
        mineTargetOrder.Start();
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
            mineTargetOrder.Update();
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
            return mineTargetOrder.OnOrderInput();
        }
    }
}
