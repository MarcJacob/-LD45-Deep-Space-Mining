using System;
using UnityEngine;
/// <summary>
/// Approach target if out of attack range. Then orbit around it.
/// </summary>
public class AttackTargetOrder : AIState
{
    private EncounterAgent target;
    private float attackRange;
    bool orbiting = false;

    public EncounterAgent Target
    {
        get { return target; }
    }

    public AttackTargetOrder(GameObject ship, float attackRange) : base(ship)
    {
        this.attackRange = attackRange;
    }

    public AttackTargetOrder(GameObject ship, EncounterAgent target, float attackRange) : base(ship)
    {
        this.attackRange = attackRange;
        AssignNewTarget(target);
    }

    public void AssignNewTarget(EncounterAgent target)
    {
        this.target = target;
        Start();
    }

    public override void Start()
    {
        orbiting = false;
    }

    public override void Update()
    {
        if (target != null)
        {
            float squaredDistToTarget = GetSquaredDistToTarget();

            orbiting = squaredDistToTarget < attackRange * attackRange;
        }
    }

    public override AIInputData OnOrderInput()
    {
        AIInputData input = new AIInputData();

        if (target != null)
        {
            if (!orbiting)
            {
                Vector2 relationVector = (target.transform.position - controlledShip.transform.position);

                input.acceleration = 1;
                relationVector = controlledShip.transform.InverseTransformDirection(relationVector);

                if (relationVector.x >= 0)
                {
                    input.turn = -1f;
                }
                else if (relationVector.x < 0)
                {
                    input.turn = 1f;
                }
            }
            else
            {
                Vector2 relationVector = (target.transform.position - controlledShip.transform.position);

                input.acceleration = 1;
                relationVector = controlledShip.transform.InverseTransformDirection(relationVector);

                if ((relationVector.x > 0 && relationVector.y > 0) || (relationVector.x < 0 && relationVector.y < 0))
                {
                    input.turn = 1f;
                }
                else if (relationVector.x != 1f && relationVector.x != -1f)
                {
                    input.turn = -1f;
                }

                // SHOOOOOOOOT
                input.usingEquipment = true;
                input.targetPosition = target.transform.position - controlledShip.transform.position;
                input.targetObject = target.gameObject;
            }
        }

        return input;
    }

    private float GetSquaredDistToTarget()
    {
        return (controlledShip.transform.position - target.transform.position).sqrMagnitude;
    }
}
