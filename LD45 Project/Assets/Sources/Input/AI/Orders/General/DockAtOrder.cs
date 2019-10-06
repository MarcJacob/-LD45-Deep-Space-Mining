using UnityEngine;

public class DockAtOrder : AIState
{
    GoToOrder goToOrder;
    private Dock target;
    private Dockable dockableComponent;
    private bool reachedTarget = false;

    public DockAtOrder(GameObject ship, Dock target) : base(ship)
    {
        this.target = target;



        goToOrder = new GoToOrder(ship, target.transform.position);
        goToOrder.OnStateFailed += GoToOrder_OnStateFailed;
        goToOrder.OnStateSucceeded += GoToOrder_OnStateSucceeded;

        dockableComponent = controlledShip.GetComponent<Dockable>();
        dockableComponent.OnShipDocked += OnShipDocked;
    }



    public DockAtOrder(GameObject ship) : base(ship)
    {
        target = null;
        goToOrder = new GoToOrder(ship);
        goToOrder.OnStateFailed += GoToOrder_OnStateFailed;
        goToOrder.OnStateSucceeded += GoToOrder_OnStateSucceeded;

        dockableComponent = controlledShip.GetComponent<Dockable>();
        dockableComponent.OnShipDocked += OnShipDocked;
    }

    public void AssignTarget(Dock t)
    {
        target = t;
        goToOrder.AssignTarget(t.transform.position);
        reachedTarget = false;
    }

    private void OnShipDocked(Dock obj)
    {
        Succeed();
    }

    public override void Start()
    {
        if (target == null)
        {
            Fail("NO TARGET");
            return;
        }

        float dist = goToOrder.GetSquaredDistanceToTarget();
        if (target.DockingRange > dist)
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
        if (target == null)
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
                interacting = true
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
