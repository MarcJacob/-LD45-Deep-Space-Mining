using System;
using UnityEngine;

/// <summary>
/// AI State for use in State Machines.
/// </summary>
public abstract class AIState
{
    public event Action OnStateSucceeded = delegate { };
    public event Action<string> OnStateFailed = delegate { };

    protected GameObject controlledShip;

    public GameObject ControlledShip
    {
        get
        {
            return controlledShip;
        }
    }

    public AIState(GameObject ship)
    {
        controlledShip = ship;
    }

    public abstract void Start();
    public abstract void Update();

    public virtual AIInputData OnOrderInput()
    {
        return new AIInputData();
    }

    protected void Succeed()
    {
        OnStateSucceeded();
    }
    public void Fail(string reason = "")
    {
        OnStateFailed(reason);
    }
}
