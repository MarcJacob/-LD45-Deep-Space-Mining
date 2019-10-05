using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager Instance;
    static public int NetWorth
    {
        get { return Instance.netWorth; }
    }
    static public int PlayerCash
    {
        get { return Instance.playerCash; }
    }
    static public void AddCash(int amount)
    {
        Instance.playerCash += amount;
    }
    static public void RemoveCash(int amount)
    {
        Instance.playerCash -= amount;
        if (Instance.playerCash < 0f)
        {
            // TODO add to debt
            Instance.playerCash = 0;
        }
    }

    [SerializeField]
    private int netWorth = 0;
    [SerializeField]
    private int playerCash = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // TOTO Calculate player networth.
        netWorth = playerCash;
    }
}
