using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Encounter
{
    static HashSet<Encounter> All = new HashSet<Encounter>();
    static public Encounter[] GetAllEncounters()
    {
        return All.ToArray();
    }

    private HashSet<EncounterAgent>[] encounterMembers = new HashSet<EncounterAgent>[Ownership.FactionCount];

    public bool EncounterOver
    {
        get; private set;
    }

    public Encounter(EncounterAgent initiator, EncounterAgent initialTarget)
    {
        for(int facID = 0; facID < Ownership.FactionCount; facID++)
        {
            encounterMembers[facID] = new HashSet<EncounterAgent>();
        }

        AddToEncounter(initiator);
        AddToEncounter(initialTarget);
        All.Add(this);
    }
    public void AddToEncounter(EncounterAgent agent)
    {
        encounterMembers[agent.GetComponent<Ownership>().OwnerFactionID].Add(agent);
        agent.EncounterJoined(this);
    }

    public void RemoveFromEncounter(EncounterAgent agent)
    {
        encounterMembers[agent.GetComponent<Ownership>().OwnerFactionID].Remove(agent);
        agent.EncounterLeft();
    }

    public List<EncounterAgent> GetEnemiesInEncounter(EncounterAgent agent)
    {
        List<EncounterAgent> enemies = new List<EncounterAgent>();
        uint[] enemyFactionIDs = agent.GetComponent<Ownership>().GetEnemyFactionIDs();
        foreach(var enemyFactionID in enemyFactionIDs)
        {
            foreach(var enemy in encounterMembers[enemyFactionID])
            {
                enemies.Add(enemy);
            }
        }

        return enemies;
    }

    internal void Update()
    {
        // If all hashsets but one are empty, then the encounter is over.
        int nonEmptyHashsets = 0;
        foreach(var hashset in encounterMembers)
        {
            if (hashset.Count > 0) nonEmptyHashsets++;
        }

        if (nonEmptyHashsets >= 2)
        {
            // Encounter isn't over.
        }
        else
        {
            // Encounter is over. Remove all current members.
            EndEncounter();
        }
    }

    public void EndEncounter()
    {
        EncounterOver = true;
        Debug.Log("Encounter ended.");
        foreach(var hashset in encounterMembers)
        {
            foreach(var member in hashset)
            {
                member.EncounterLeft();
            }
        }
        All.Remove(this);
    }
}
