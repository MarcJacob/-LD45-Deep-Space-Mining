﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager Instance;
    static public float NetWorth
    {
        get
        {
            return Instance.playerCash - Instance.playerDebt + Instance.playerAssetsValue;
        } // TODO Get all player owned ships and add their value aswell as their cargo's base value. }
    }
    static public float PlayerCash
    {
        get { return Instance.playerCash; }
    }
    static public float Debt
    {
        get { return Instance.playerDebt; }
    }
    static public float GracePeriod
    {
        get { return Instance.gracePeriod; }
    }



    public static void RemoveDebt(float currentLoanPayBackValue)
    {
        Instance.playerDebt -= currentLoanPayBackValue;
    }

    static public void AddCash(float amount)
    {
        Instance.playerCash += amount;
    }
    static public void RemoveCash(float amount)
    {
        Instance.playerCash -= amount;
        if (Instance.playerCash < 0f)
        {
            // TODO add to debt
            Instance.playerCash = 0;
        }
    }
    static public void AddDebt(float amount)
    {
        Instance.playerDebt += amount;
    }
    static public void PaybackDebt(float amount)
    {
        Instance.playerDebt -= amount;
    }

    [SerializeField]
    private float playerCash = 0;
    [SerializeField]
    private float playerDebt = 0;
    [SerializeField]
    private float debtInterest = 0.1f;
    [SerializeField]
    private float debtIncreaseCooldown = 60f;
    [SerializeField]
    private float gracePeriod = 300f;
    [SerializeField]
    private float goalNetWorth = 100000f;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject victoryScreen;

    private float currentDebtIncreaseCooldown = 0f;
    private bool gracePeriodStarted = false;

    public float playerAssetsValue { get
        {
            var allFacMembers = Ownership.GetFactionMembers(1);
            float val = 0f;
            foreach(var member in allFacMembers)
            {
                val += member.GetComponent<ShipProperties>().Price;
                val += member.GetComponent<Cargo>().CargoBaseValue;
            }
            return val;
        }
    }

    public void StartGracePeriod()
    {
        gracePeriodStarted = true;
    }

    private void Awake()
    {
        Instance = this;
        currentDebtIncreaseCooldown = debtIncreaseCooldown;
    }

    bool victoryScreenShown = false;
    private void Update()
    {
        if (gracePeriodStarted) gracePeriod -= Time.deltaTime;
        if (Debt > 0)
        {
            currentDebtIncreaseCooldown -= Time.deltaTime;
            if (currentDebtIncreaseCooldown < 0f)
            {
                playerDebt *= 1f + debtInterest;
                currentDebtIncreaseCooldown = debtIncreaseCooldown;
            }

            if (gracePeriod < 0f && NetWorth < -10000f)
            {
                Debug.Log("GameOver");
                gameOverScreen.SetActive(true);
            }
        }

        if (!victoryScreenShown && NetWorth > goalNetWorth)
        {
            Debug.Log("Game won !");
            victoryScreenShown = true;
            victoryScreen.SetActive(true);
        }
    }

}
