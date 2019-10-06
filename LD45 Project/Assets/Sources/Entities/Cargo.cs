using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Data holding component.
/// Determines how much of each resources is within the cargo.
/// </summary>
public class Cargo : MonoBehaviour
{
    [SerializeField]
    private uint cargoCapacity;

    [SerializeField]
    private uint[] cargo;

    public uint[] StoredResources
    {
        get
        {
            return cargo;
        }
    }
    public uint StoredAmount
    {
        get { return (uint)cargo.Sum(c => c); }
    }
    public uint Capacity
    {
        get { return cargoCapacity; }
    }

    private void Awake()
    {
        uint[] finalCargoArray = new uint[Enum.GetNames(typeof(RESOURCE_TYPE)).Length];
        if (cargo == null)
            cargo = finalCargoArray;
        else
        {
            for(int i = 0; i < cargo.Length && i < finalCargoArray.Length; i++)
            {
                finalCargoArray[i] = cargo[i];
            }
            cargo = finalCargoArray;
        }
    }

    public bool AddResource(uint id, uint amount)
    {
        uint t;
        return AddResource(id, amount, out t);
    }
    public bool AddResource(uint id, uint amount, out uint added)
    {
        uint roomLeftInCargo = cargoCapacity - (uint)cargo.Sum(c => c);
        if (roomLeftInCargo >= amount)
        {
            cargo[id] += amount;
            added = amount;
            return true;
        }
        else
        {
            cargo[id] += roomLeftInCargo;
            added = roomLeftInCargo;
            return false;
        }
    }

    /// <summary>
    /// Withdraws resources from cargo. If not enough resources are available, withdraws all that are left.
    /// </summary>
    public bool WithdrawResourceToMax(uint id, uint amount, out uint withdrawn)
    {
        if (cargo[id] >= amount)
        {
            withdrawn = amount;
            cargo[id] -= withdrawn;
            return true;
        }
        else
        {
            withdrawn = cargo[id];
            cargo[id] = 0;
            return false;
        }
    }

    public bool IsEmpty()
    {
        return cargo.Sum(c => c) == 0;
    }

    public bool IsFull
    {
        get
        {
            long currentCargo = cargo.Sum(c => c);
            return currentCargo >= cargoCapacity;
        }
    }

    public bool HasResource(uint id)
    {
        return cargo[id] > 0;
    }

    public uint[] GetStoredResourceTypes()
    {
        List<uint> storedTypes = new List<uint>();
        for(uint i = 0; i < cargo.Length; i++)
        {
            if (cargo[i] > 0) storedTypes.Add(i);
        }
        return storedTypes.ToArray();
    }
}
