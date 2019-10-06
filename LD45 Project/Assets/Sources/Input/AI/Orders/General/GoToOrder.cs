using UnityEngine;

public class GoToOrder : AIState
{
    Vector2? targetPosition;
    float arrivalDistance;

    public GoToOrder(GameObject ship, Vector2 targetPosition, float arrivalDistance = 5f) : base(ship) // TODO always pass a universal position rather than a Unity position.
    {
        this.targetPosition = targetPosition;
        this.arrivalDistance = arrivalDistance;
    }

    public GoToOrder(GameObject ship, float arrivalDistance = 5f) : base(ship)
    {
        targetPosition = null;
        this.arrivalDistance = arrivalDistance;
    }

    public void AssignTarget(Vector2 t)
    {
        targetPosition = t;
    }

    public override void Start()
    {
        if (targetPosition.HasValue == false)
        {
            Fail("NO TARGET");
            return;
        }
    }

    public override void Update()
    {
        Debug.DrawLine(controlledShip.transform.position, targetPosition.Value, Color.blue);
        float dist = GetSquaredDistanceToTarget();
        
        if (dist <= arrivalDistance * arrivalDistance)
        {
            Succeed();
        }
    }

    public override AIInputData OnOrderInput()
    {
        AIInputData input = new AIInputData();
        Vector2 relationVector = (targetPosition.Value - (Vector2)controlledShip.transform.position);

        input.acceleration = 1f - Mathf.Lerp(0f, 2f, 1f - (relationVector.sqrMagnitude / (arrivalDistance * arrivalDistance) * 2f));
        relationVector = controlledShip.transform.InverseTransformDirection(relationVector);

        if (relationVector.x >= 0)
        {
            input.turn = -1f;
        }
        else if (relationVector.x < 0)
        {
            input.turn = 1f;
        }

        return input;
    }

    public float GetSquaredDistanceToTarget() // TODO return something that uses our custom coordinates system.
    {
        return ((Vector2)controlledShip.transform.position - targetPosition.Value).sqrMagnitude;
    }
}
