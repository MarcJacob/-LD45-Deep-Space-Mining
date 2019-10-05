using UnityEngine;
using TMPro;

public class TradePanelLine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemAmountText;
    [SerializeField]
    private TextMeshProUGUI itemPricePerUnitText;

    RESOURCE_TYPE resourceType;
    uint amount;
    float pricePerUnit;

    public int ResourceID
    {
        get
        {
            return (int)resourceType;
        }
    }
    public uint Amount
    {
        get
        {
            return amount;
        }
    }
    public float PricePerUnit
    {
        get
        {
            return pricePerUnit;
        }
    }

    public void SetLineInfo(RESOURCE_TYPE resourceType, uint amount, float pricePerUnit)
    {
        this.resourceType = resourceType;
        this.amount = amount;
        this.pricePerUnit = pricePerUnit;

        itemNameText.text = this.resourceType.GetDescription();
        itemAmountText.text = "x" + amount;
        itemPricePerUnitText.text = pricePerUnit + "$";
    }

    public void OnLineSelected()
    {
        itemNameText.color = Color.red;
        itemAmountText.color = Color.red;
        itemPricePerUnitText.color = Color.red;
    }

    public void OnLineDeselected()
    {
        itemNameText.color = Color.white;
        itemAmountText.color = Color.white;
        itemPricePerUnitText.color = Color.white;
    }
}
