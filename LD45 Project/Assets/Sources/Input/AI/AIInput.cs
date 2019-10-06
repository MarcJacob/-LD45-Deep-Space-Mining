using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class AIInput : EntityInput
{
    private Dockable dockableComponent;

    MineAndSellAutomaticallyBehavior currentBehavior;
    AIOrderQueue orderQueue = new AIOrderQueue();

    private void Awake()
    {
        dockableComponent = GetComponent<Dockable>();
    }

    private void Start()
    {
        dockableComponent.Undock();

        currentBehavior = new MineAndSellAutomaticallyBehavior(gameObject, orderQueue);
        currentBehavior.StartBehavior();
    }

    private void Update()
    {
        currentBehavior.UpdateBehavior();
        orderQueue.UpdateCurrentOrder();

        if (orderQueue.CurrentOrder != null)
        {
            ApplyInput(orderQueue.CurrentOrder.OnOrderInput());
        }
        else
        {
            ApplyInput(new AIInputData());
        }
    }

    private void ApplyInput(AIInputData orderInput)
    {
        acceleration = orderInput.acceleration;
        turningDirection = orderInput.turn;
        usingActiveEquipment = orderInput.usingEquipment;
        targetedObject = orderInput.targetObject;
        targetedPosition = orderInput.targetPosition;
        interacting = orderInput.interacting;
    }
}

public struct AIInputData
{
    public float acceleration;
    public float turn;
    public float strafe;
    public bool interacting;
    public bool usingEquipment;
    public UnityEngine.Vector2 targetPosition;
    public GameObject targetObject;
}

public class AIOrderQueue
{
    private List<AIState> queue = new List<AIState>();
    private bool startOrderOnSwitching = true;
    public AIState CurrentOrder
    {
        get
        {
            return queue.Count > 0 ? queue[0] : null;
        }
    }

    public void AssignOrder(AIState order, bool setAsCurrentOrder = false, bool failCurrentOrder = false)
    {
        if (setAsCurrentOrder)
        {
            startOrderOnSwitching = false;
            if (CurrentOrder != null && failCurrentOrder)
            {
                CurrentOrder.Fail("ORDER OVERWRITTEN");
            }
            queue.Insert(0, order);

            queue[0].OnStateFailed += OnCurrentOrderFailed;
            queue[0].OnStateSucceeded += OnCurrentOrderSucceeded;

            queue[0].Start();

            startOrderOnSwitching = true;
        }
        else
        {
            queue.Add(order);
            if (queue.Count == 1) // If this is true, then the order we've just added is the CurrentOrder.
            {
                queue[0].OnStateFailed += OnCurrentOrderFailed;
                queue[0].OnStateSucceeded += OnCurrentOrderSucceeded;
                queue[0].Start();
            }
        }
    }

    public void UpdateCurrentOrder()
    {
        if (CurrentOrder != null)
        {
            CurrentOrder.Update();
        }
    }

    private void OnCurrentOrderSucceeded()
    {
        CurrentOrder.OnStateFailed -= OnCurrentOrderFailed;
        CurrentOrder.OnStateSucceeded -= OnCurrentOrderSucceeded;
        SwitchToNextOrder();
    }

    private void SwitchToNextOrder()
    {
        queue.RemoveAt(0);
        // Now Current Order is actually the next order in the queue !
        if (startOrderOnSwitching && CurrentOrder != null)
        {
            CurrentOrder.OnStateFailed += OnCurrentOrderFailed;
            CurrentOrder.OnStateSucceeded += OnCurrentOrderSucceeded;
            CurrentOrder.Start();
        }
    }

    private void OnCurrentOrderFailed(string reason)
    {
        CurrentOrder.OnStateFailed -= OnCurrentOrderFailed;
        CurrentOrder.OnStateSucceeded -= OnCurrentOrderSucceeded;

        UnityEngine.Debug.Log("AI " + CurrentOrder.ControlledShip.name + " failed order of type " + CurrentOrder.GetType().ToString() + " because : " + reason);
        SwitchToNextOrder();
    }

    public void ClearQueue()
    {
        if (CurrentOrder != null) CurrentOrder.Fail("ORDER OVERWRITTEN");
        queue.Clear();
    }

}