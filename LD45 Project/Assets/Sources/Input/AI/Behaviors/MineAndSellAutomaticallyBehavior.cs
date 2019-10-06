using System;
using UnityEngine;
public class MineAndSellAutomaticallyBehavior
{
    GameObject controlledShip;
    AIOrderQueue orderQueue;

    MineNearest mineNearestOrder;
    DockAtNearestOrder dockAtNearestOrder;
    SellCargoToDockedStation sellToDockedStationOrder; // TODO make "sell cargo at nearest" which combines the two.
    UndockOrder undockOrder;
    bool ongoing = false; // Are there any orders related to this behavior currently running ?

    public MineAndSellAutomaticallyBehavior(GameObject ship, AIOrderQueue orderQueue)
    {
        controlledShip = ship;
        this.orderQueue = orderQueue;
        mineNearestOrder = new MineNearest(ship);
        dockAtNearestOrder = new DockAtNearestOrder(ship);
        sellToDockedStationOrder = new SellCargoToDockedStation(ship);
        undockOrder = new UndockOrder(ship);
        mineNearestOrder.OnStateSucceeded += OnOrderFinished;
        mineNearestOrder.OnStateFailed += (str) => OnOrderFinished();
        sellToDockedStationOrder.OnStateSucceeded += OnOrderFinished;
    }

    private void OnOrderFinished()
    {
        ongoing = false;
    }

    public void StartBehavior()
    {
        ongoing = true;
        orderQueue.AssignOrder(mineNearestOrder);
    }

    public void UpdateBehavior()
    {
        if (ongoing == false)
        {
            if (controlledShip.GetComponent<Cargo>().IsFull && controlledShip.GetComponent<Dockable>().Docked == false)
            {
                orderQueue.AssignOrder(dockAtNearestOrder);
                orderQueue.AssignOrder(sellToDockedStationOrder);
            }
            else
            {
                orderQueue.AssignOrder(undockOrder);
                orderQueue.AssignOrder(mineNearestOrder);
            }
            ongoing = true;
        }
    }
}
