using UnityEngine;

public class UndockOrder : AIState
{
    Dockable dockableComponent;

    public UndockOrder(GameObject ship) : base(ship)
    {
        dockableComponent = ship.GetComponent<Dockable>();
        dockableComponent.OnShipUndocked += DockableComponent_OnShipUndocked;
    }

    private void DockableComponent_OnShipUndocked(Dock obj)
    {
        Succeed();
    }

    public override void Start()
    {
        if (dockableComponent.Docked == false) Succeed();
    }

    public override void Update()
    {
        
    }

    public override AIInputData OnOrderInput()
    {
        if (dockableComponent.Docked)
            return new AIInputData
            {
                interacting = true
            };
        else
            return base.OnOrderInput();
    }
}
