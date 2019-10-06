using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ownership : MonoBehaviour
{
    const int FactionCount = 0; // Number of NPC factions beyond 0 (Unowned), 1 (Player) and 2 (Pirates).
    static HashSet<Ownership>[] FactionMembers = new HashSet<Ownership>[3 + FactionCount];

    static Ownership()
    {
        for(int i = 0; i < FactionMembers.Length; i++)
        {
            FactionMembers[i] = new HashSet<Ownership>();
        }
    }

    static public Ownership[] GetFactionMembers(int fac)
    {
        return FactionMembers[fac].ToArray();
    }

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
            FactionMembers[ownerFactionID].Remove(this);
            ownerFactionID = value;
            FactionMembers[ownerFactionID].Add(this);
            OnOwnerChanged(ownerFactionID);
        }
    }
}
