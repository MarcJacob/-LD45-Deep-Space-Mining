using UnityEngine;
/// <summary>
/// Mines specified target until cargo full. Will go to target automatically if it is not within range.
/// </summary>
public class MineTarget : AIState
{
    GoToOrder goToOrder;
    private Mineable target;
    private Cargo shipCargo;
    private bool reachedTarget = false;
    private float miningRange;

    public MineTarget(GameObject ship, Mineable target) : base(ship)
    {
        this.target = target;
        miningRange = controlledShip.GetComponent<MiningBeam>().Range;

        goToOrder = new GoToOrder(ship, target.transform.position, miningRange);
        goToOrder.OnStateFailed += GoToOrder_OnStateFailed;
        goToOrder.OnStateSucceeded += GoToOrder_OnStateSucceeded;
    }

    public MineTarget(GameObject ship) : base(ship)
    {
        target = null;
        goToOrder = new GoToOrder(ship);
        goToOrder.OnStateFailed += GoToOrder_OnStateFailed;
        goToOrder.OnStateSucceeded += GoToOrder_OnStateSucceeded;
    }

    public void AssignTarget(Mineable t)
    {
        target = t;
        goToOrder.AssignTarget(t.transform.position);
        reachedTarget = false;
    }

    public override void Start()
    {
        shipCargo = controlledShip.GetComponent<Cargo>();
        if (target == null)
        {
            Fail("NO TARGET");
            return;
        }

        float dist = goToOrder.GetSquaredDistanceToTarget();
        if (miningRange * miningRange > dist)
        {
            reachedTarget = true;
        }
        else
        {
            reachedTarget = false;
            goToOrder.Start();
        }
    }

    public override void Update()
    {
        if (shipCargo.IsFull || (target == null && reachedTarget))
        {
            Succeed();
            Debug.Log("Mined asteroid !");
        }
        else if (target == null && !reachedTarget)
        {
            Fail("TARGET DESTROYED");
        }
        else if (!reachedTarget)
        {
            goToOrder.Update();
        }
    }

    public override AIInputData OnOrderInput()
    {
        if (reachedTarget && target != null)
        {
            return new AIInputData
            {
                usingEquipment = true,
                targetPosition = target.transform.position - controlledShip.transform.position,
                targetObject = target.gameObject
            };
        }
        else
        {
            return goToOrder.OnOrderInput();
        }
    }

    


    private void GoToOrder_OnStateSucceeded()
    {
        reachedTarget = true;
    }

    private void GoToOrder_OnStateFailed(string obj)
    {
        goToOrder.Start();
    }
}