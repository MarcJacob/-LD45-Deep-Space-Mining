using UnityEngine;

public class SellCargoToDockedStation : AIState
{
    private Dockable dockableComponent;
    private Cargo shipCargoComponent;

    public SellCargoToDockedStation(GameObject ship) : base(ship)
    {
        dockableComponent = ship.GetComponent<Dockable>();
        shipCargoComponent = ship.GetComponent<Cargo>();
    }

    public override void Start()
    {
        if (dockableComponent == null || dockableComponent.Docked == false)
        {
            Fail("SHIP NOT DOCKED");
            return;
        }

        var tradingComponent = dockableComponent.DockedAt.GetComponent<Trading>();
        if (tradingComponent != null)
        {
            var storedResourcesID = shipCargoComponent.GetStoredResourceTypes();
            for (int i = 0; i < storedResourcesID.Length; i++)
            {
                uint resourceTypeID = storedResourcesID[i];
                RESOURCE_TYPE t = (RESOURCE_TYPE)resourceTypeID;
                tradingComponent.BuyFrom(shipCargoComponent, t, shipCargoComponent.StoredResources[resourceTypeID], t.GetBasePrice()); // TODO Dynamic prices
            }
            Succeed();
        }
        else
        {
            Fail("STATION HAS NO TRADING COMPONENT");
            return;
        }
    }

    public override void Update()
    {

    }
}
