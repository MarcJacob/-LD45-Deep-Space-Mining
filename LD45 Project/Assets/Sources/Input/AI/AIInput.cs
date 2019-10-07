using System.Numerics;

public class AIInput : EntityInput
{
    AIBehavior currentBehavior;
    AIOrderQueue orderQueue = new AIOrderQueue();

    private void Awake()
    {
    }

    private void Start()
    {
        if (GetComponent<MiningBeam>())
            currentBehavior = new MineAndSellAutomaticallyBehavior(gameObject, orderQueue);
        else
            currentBehavior = new DefendAreaBehavior(new UnityEngine.Vector2(0, 0), 100f, gameObject, orderQueue);
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
