using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoanManagementUI : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField payBackValueInputField;
    [SerializeField]
    private TextMeshProUGUI gracePeriodText;

    private float currentLoanPayBackValue = 0f;

    public void TakeLoan(float loanAmount)
    {
        GameManager.AddCash(loanAmount);
        loanAmount *= 1.1f;
        GameManager.AddDebt(loanAmount);
    }

    public void PayBackLoan()
    {
        if (currentLoanPayBackValue > 0f)
        {
            GameManager.RemoveCash(currentLoanPayBackValue);
            GameManager.RemoveDebt(currentLoanPayBackValue);
            UpdateLoanPayBackValue(currentLoanPayBackValue.ToString());
        }
    }

    public void UpdateLoanPayBackValue(string val)
    {
        if (val.Length == 0) return;
        currentLoanPayBackValue = float.Parse(val);
        currentLoanPayBackValue = Mathf.Min(currentLoanPayBackValue, GameManager.Debt);
        currentLoanPayBackValue = Mathf.Min(currentLoanPayBackValue, GameManager.PlayerCash);

        payBackValueInputField.text = currentLoanPayBackValue.ToString();
    }

    private void Update()
    {
        string minuteFormat = ""+(int)(GameManager.GracePeriod / 60);
        string secondsFormat = "" + (int)(GameManager.GracePeriod % 60);
        if (minuteFormat.Length == 1) minuteFormat = "0" + minuteFormat;
        if (secondsFormat.Length == 1) secondsFormat = "0" + secondsFormat;
    }
}
