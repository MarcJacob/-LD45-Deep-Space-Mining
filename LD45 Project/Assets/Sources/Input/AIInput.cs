public class AIInput : EntityInput
{
    private Dockable dockableComponent;

    MineAndSellAutomaticallyStateMachine currentOrder;

    private void Awake()
    {
        dockableComponent = GetComponent<Dockable>();
    }

    private void Start()
    {
        currentOrder.StartOrder();
    }

    private void Update()
    {
        if (dockableComponent.Docked)
        {
            dockableComponent.Undock();
        }
        else
        {
            currentOrder.UpdateOrder();
        }
    }
}
