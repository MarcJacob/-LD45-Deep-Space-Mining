using UnityEngine;

public class ShipPiloting : MonoBehaviour
{
    [SerializeField]
    EntityInput currentPilot;
    private Ownership ownershipComponent;

    public EntityInput CurrentPilot
    {
        get
        {
            return currentPilot;
        }
    }

    public bool PlayerControlled
    {
        get
        {
            return CurrentPilot != null && CurrentPilot is PlayerInput;
        }
    }

    public bool AIControlled
    {
        get
        {
            return CurrentPilot != null && CurrentPilot is AIInput;
        }
    }

    public void SwitchToPlayerControl()
    {
        if (currentPilot != null) currentPilot.Disable();
        currentPilot = GetComponent<PlayerInput>();
        currentPilot.enabled = true;
    }

    public void SwitchToAIControl()
    {
        if (currentPilot != null) currentPilot.Disable();
        currentPilot = GetComponent<AIInput>();
        currentPilot.enabled = true;
    }

    public void SwitchToNoControl()
    {
        if (currentPilot != null) currentPilot.Disable();
        currentPilot = null;
    }

    private void Awake()
    {
        ownershipComponent = GetComponent<Ownership>();
        if (ownershipComponent != null)
        {
            ownershipComponent.OnOwnerChanged += OwnershipComponent_OnOwnerChanged;
        }
    }

    private void OwnershipComponent_OnOwnerChanged(uint newFaction)
    {
        if (newFaction == 0)
        {
            SwitchToNoControl();
        }
        else if (newFaction > 1)
        {
            SwitchToAIControl();
        }
    }
}