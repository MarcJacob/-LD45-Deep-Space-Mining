using System;
using UnityEngine;

public abstract class AIBehavior
{
    protected GameObject controlledShip;
    protected AIOrderQueue orderQueue;
    public AIBehavior(GameObject ship, AIOrderQueue orderQueue)
    {
        controlledShip = ship;
        this.orderQueue = orderQueue;
    }

    public abstract void StartBehavior();
    public abstract void UpdateBehavior();

    public abstract void StopBehavior();
}

public class MineAndSellAutomaticallyBehavior : AIBehavior
{
    MineNearest mineNearestOrder;
    DockAtNearestOrder dockAtNearestOrder;
    SellCargoToDockedStation sellToDockedStationOrder; // TODO make "sell cargo at nearest" which combines the two.
    UndockOrder undockOrder;
    bool ongoing = false; // Are there any orders related to this behavior currently running ?

    public MineAndSellAutomaticallyBehavior(GameObject ship, AIOrderQueue orderQueue) : base(ship, orderQueue)
    {
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

    public override void StartBehavior()
    {

    }

    public override void UpdateBehavior()
    {
        if (ongoing == false)
        {
            if (controlledShip.GetComponent<Cargo>().IsFull && controlledShip.GetComponent<Dockable>().Docked == false)
            {
                orderQueue.AssignOrder(undockOrder);
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

    public override void StopBehavior()
    {
        
    }
}
