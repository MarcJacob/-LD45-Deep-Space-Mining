using UnityEngine;
using TMPro;

public class ShipPanelLine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI shipNameText;
    [SerializeField]
    private TextMeshProUGUI shipSizeText;
    [SerializeField]
    private TextMeshProUGUI shipPriceText;

    GameObject ship;
    uint size;
    float price;
    bool playerShip = false;
    bool aiShip = false;

    private bool selected = false;

    public GameObject Ship
    {
        get
        {
            return ship;
        }
    }
    public uint Size
    {
        get
        {
            return size;
        }
    }
    public float Price
    {
        get
        {
            return price;
        }
    }

    public void SetLineInfo(GameObject ship, uint amount, float pricePerUnit)
    {
        this.ship = ship;
        this.size = amount;
        this.price = pricePerUnit;

        shipNameText.text = ship.name;
        shipSizeText.text = Size.ToString();
        shipPriceText.text = pricePerUnit + "$";

        playerShip = ship.GetComponent<ShipPiloting>().PlayerControlled;
        aiShip = ship.GetComponent<ShipPiloting>().AIControlled;
        if (!selected)
            RefreshColor();
    }

    public void OnLineSelected()
    {
        shipNameText.color = Color.red;
        shipSizeText.color = Color.red;
        shipPriceText.color = Color.red;
        selected = true;
    }

    public void OnLineDeselected()
    {
        selected = false;
        RefreshColor();
    }

    private void RefreshColor()
    {
        if (playerShip)
        {
            shipNameText.color = Color.green;
            shipSizeText.color = Color.green;
            shipPriceText.color = Color.green;

        }
        else if (aiShip)
        {
            shipNameText.color = Color.green * 0.8f;
            shipSizeText.color = Color.green * 0.8f;
            shipPriceText.color = Color.green * 0.8f;
        }
        else
        {
            shipNameText.color = Color.white;
            shipSizeText.color = Color.white;
            shipPriceText.color = Color.white;
        }
    }
}