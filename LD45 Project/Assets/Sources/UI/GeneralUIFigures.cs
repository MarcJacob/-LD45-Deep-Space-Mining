using System.Collections;
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
        playerNetWorthText.text = "$ " + ((int)(GameManager.NetWorth)).ToString();
        playerCashText.text = "$ " + (int)GameManager.PlayerCash;
        playerDebtText.text = "$ " + (int)GameManager.Debt;
    }
}
