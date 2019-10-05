using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ownership : MonoBehaviour
{
    [SerializeField]
    private uint ownerFactionID; // 0 = Unowned, 1 = Player faction, 2 = Pirates, > 2 = NPC factions (whatever they may be).

    public event Action<uint> OnOwnerChanged = delegate { };
    
    public uint OwnerFactionID
    {
        get
        {
            return ownerFactionID;
        }
        set
        {
            ownerFactionID = value;
            OnOwnerChanged(ownerFactionID);
        }
    }
}
