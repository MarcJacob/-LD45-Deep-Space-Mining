using UnityEngine;

public class DockAtOrder : AIState
{
    const float PostDockSuccessMaximumDelay = 6f;

    GoToOrder goToOrder;
    private Dock target;
    private Dockable dockableComponent;
    private bool reachedTarget = false;
    private bool docked = true;
    private float currentPostDockSuccessDelay = 0f;

    public DockAtOrder(GameObject ship, Dock target) : base(ship)
    {
        this.target = target;



        goToOrder = new GoToOrder(ship, target.transform.position, target.DockingRange * 0.8f);
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
        goToOrder.AssignTarget(t.transform.position, target.DockingRange * 0.8f);
        reachedTarget = false;
    }

    private void OnShipDocked(Dock obj)
    {
        docked = true;
    }

    public override void Start()
    {
        if (target == null)
        {
            Fail("NO TARGET");
            return;
        }
        currentPostDockSuccessDelay = Random.Range(0f, PostDockSuccessMaximumDelay * 0.8f);
        docked = false;
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
        else if (docked)
        {
            currentPostDockSuccessDelay += Time.deltaTime;
            if (currentPostDockSuccessDelay > PostDockSuccessMaximumDelay)
                Succeed();
        }
    }

    public override AIInputData OnOrderInput()
    {
        if (!docked && reachedTarget && target != null)
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
