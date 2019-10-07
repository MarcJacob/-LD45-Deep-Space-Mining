using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Ownership))]
public class EncounterAgent : MonoBehaviour
{
    Encounter currentEncounter;

    public Encounter CurrentEncounter {
        get
        {
            return currentEncounter;
        }
    }

    public event Action<Encounter> OnEncounterJoined = delegate { };
    public event Action<Encounter> OnEncounterLeft = delegate { };

    public void Awake()
    {
        var dockableComponent = GetComponent<Dockable>();
        if (dockableComponent != null)
        {
            dockableComponent.OnShipDocked += DockableComponent_OnShipDocked;
        }
    }

    private void DockableComponent_OnShipDocked(Dock obj)
    {
        if (CurrentEncounter != null) CurrentEncounter.RemoveFromEncounter(this);
    }

    public void StartEncounter(EncounterAgent otherAgent)
    {
        new Encounter(this, otherAgent);
    }

    public void EncounterLeft()
    {
        var e = currentEncounter;
        currentEncounter = null;
        OnEncounterLeft(e);
    }

    public void EncounterJoined(Encounter encounter)
    {
        if (currentEncounter != null)
        {
            currentEncounter.RemoveFromEncounter(this);
        }
        currentEncounter = encounter;
        OnEncounterJoined(encounter);
    }

    private void OnDestroy()
    {
        if (CurrentEncounter != null)
        {
            CurrentEncounter.RemoveFromEncounter(this);
        }
    }

    private void OnDisable()
    {
        if (CurrentEncounter != null)
        {
            CurrentEncounter.RemoveFromEncounter(this);
        }
    }
}
