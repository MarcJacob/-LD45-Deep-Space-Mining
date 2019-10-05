using System;
using UnityEngine;

public abstract class IAIStateMachineState
{
    public event Action OnStateFinished = delegate { };
    public event Action<string> OnStateFailed = delegate { };

    protected GameObject controlledShip;

    public IAIStateMachineState(GameObject ship)
    {
        controlledShip = ship;
    }

    public abstract void Start();
    public abstract void Update();

    void Finish()
    {
        OnStateFinished();
    }
    public void Fail(string reason = "")
    {
        OnStateFailed(reason);
    }
}
