using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Objects with this script can have ships docked inside of them.
/// </summary>
public class Dock : MonoBehaviour
{
    [SerializeField]
    private Dockable[] dockedShipsVisible;
    [SerializeField]
    private float dockingRange = 1f;

    public float DockingRange { get
        {
            return dockingRange;
        }
    }

    private HashSet<Dockable> dockedShips = new HashSet<Dockable>();

    public Dockable[] DockedShips
    {
        get
        {
            return dockedShips.ToArray();
        }
    }

    private void Update()
    {
        dockedShipsVisible = dockedShips.ToArray();
    }

    public bool DockShip(Dockable ship)
    {
        dockedShips.Add(ship);
        return true;
    }

    public void UndockShip(Dockable ship)
    {
        dockedShips.Remove(ship);
    }
}
