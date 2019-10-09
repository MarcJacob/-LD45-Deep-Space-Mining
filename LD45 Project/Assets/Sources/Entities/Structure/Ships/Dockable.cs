using System;
using UnityEngine;

[RequireComponent(typeof(EntityInput))]
public class Dockable : MonoBehaviour
{
    public event Action<Dock> OnShipDocked = delegate { };
    public event Action<Dock> OnShipUndocked = delegate { };
    public event Action<Dock> OnShipInDockingRange = delegate { };
    public event Action OnShipOutOfDockingRange = delegate { };

    [SerializeField]
    private GameObject[] disableOnDock;
    [SerializeField]
    private GameObject[] enableOnUndock;

    private Dock currentDockInteractionCandidate;
    private ShipPiloting shipControllerInput;

    public bool Docked { get; private set; }
    public Dock DockedAt { get
        {
            if (!Docked) return null;
            else return currentDockInteractionCandidate;
        }
    }

    private void Awake()
    {
        shipControllerInput = GetComponent<ShipPiloting>();
    }

    private void Start()
    {
        InvokeRepeating("CheckForDocksInRange", 0f, 0.5f);
    }

    public void AskDock(Dock dock)
    {
        if (Vector3.Distance(transform.position, dock.transform.position) <= dock.DockingRange && dock.DockShip(this))
        {
            Docked = true;
            currentDockInteractionCandidate = dock;
            OnShipDocked(dock);
            foreach(var go in disableOnDock)
            {
                go.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("WARNING - The station is either full or not in range to dock.");
        }
    }

    public void Undock()
    {
        if (currentDockInteractionCandidate != null)
        {
            currentDockInteractionCandidate.UndockShip(this);
            currentDockInteractionCandidate = null;

            OnShipUndocked(currentDockInteractionCandidate);
            Docked = false;
            foreach (var go in enableOnUndock)
            {
                go.SetActive(true);
            }
        }
    }

    private void CheckForDocksInRange()
    {
        if (shipControllerInput.CurrentPilot != null)
        {
            var docks = GameObject.FindObjectsOfType<Dock>();
            float shortestRange = -1f;
            Dock nearest = null;

            foreach (var dock in docks)
            {
                // TODO use custom grid based position system
                float dist = (transform.position - dock.transform.position).sqrMagnitude;
                if (dist < shortestRange || nearest == null)
                {
                    nearest = dock;
                    shortestRange = dist;
                }
            }

            if (nearest != null && Mathf.Sqrt(shortestRange) <= nearest.DockingRange)
            {
                OnShipInDockingRange(nearest);
                currentDockInteractionCandidate = nearest;
            }
        }
    }

    private void Update()
    {
        if (shipControllerInput.CurrentPilot != null && currentDockInteractionCandidate != null)
        {
            float dist = Vector3.Distance(transform.position, currentDockInteractionCandidate.transform.position);
            if (dist > currentDockInteractionCandidate.DockingRange)
            {
                currentDockInteractionCandidate = null;
                OnShipOutOfDockingRange();
            }
            else if (Docked == false && shipControllerInput.CurrentPilot.interacting)
            {
                AskDock(currentDockInteractionCandidate);
            }
            else if (Docked && shipControllerInput.CurrentPilot.interacting)
            {
                Undock();
            }
        }
    }
}
