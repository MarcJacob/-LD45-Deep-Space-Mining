﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneralUIFigures : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNetWorthText;
    [SerializeField]
    private TextMeshProUGUI playerCashText;
    [SerializeField]
    private TextMeshProUGUI playerDebtText;

    private void Update()
    {
        playerNetWorthText.text = "$ " + GameManager.NetWorth.ToString();
        playerCashText.text = "$ " + GameManager.PlayerCash;
        playerDebtText.text = "$ " + GameManager.Debt;
    }
}
