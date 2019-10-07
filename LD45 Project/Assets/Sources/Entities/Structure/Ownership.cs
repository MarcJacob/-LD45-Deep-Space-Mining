using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ownership : MonoBehaviour
{
    const int NPCFactionCount = 0; // Number of NPC factions beyond 0 (Unowned), 1 (Player) and 2 (Pirates).
    static HashSet<Ownership>[] FactionMembers = new HashSet<Ownership>[FactionCount];

    static public int FactionCount
    {
        get
        {
            return 3 + NPCFactionCount;
        }
    }

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

    private void Awake()
    {
        OwnerFactionID = ownerFactionID;
    }

    private void OnDestroy()
    {
        FactionMembers[OwnerFactionID].Remove(this);
    }

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

    public bool IsEnemy(Ownership other)
    {
        // TODO Diplomatic standings ?
        return other.ownerFactionID > 0 && other.ownerFactionID != ownerFactionID;
    }

    public uint[] GetEnemyFactionIDs()
    {
        // TODO Diplomatic standings ?
        List<uint> ids = new List<uint>();
        for(uint facID = 1; facID < FactionCount; facID++)
        {
            if (facID != ownerFactionID) ids.Add(facID);
        }

        return ids.ToArray();
    }
}
