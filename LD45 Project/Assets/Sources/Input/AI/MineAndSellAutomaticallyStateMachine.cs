using UnityEngine;
public class MineAndSellAutomaticallyStateMachine
{
    IAIStateMachineState currentState;

    public void StartOrder()
    {

    }

    public void UpdateOrder()
    {
        currentState.Update();
    }
}

public class SeekMineableAsteroidState : IAIStateMachineState
{
    public SeekMineableAsteroidState(GameObject ship) : base(ship)
    {
    }

    public override void Start()
    {
        Mineable[] mineableObjects = GameObject.FindObjectsOfType<Mineable>();
        float closestDist = -1f;
        Mineable closestMineable = null;

        foreach(var obj in mineableObjects)
        {
            float dist = (controlledShip.transform.position - obj.transform.position).sqrMagnitude;
            if (closestMineable == null || dist < closestDist)
            {
                closestMineable = obj;
                closestDist = dist;
            }
        }


    }

    public override void Update()
    {
    }
}