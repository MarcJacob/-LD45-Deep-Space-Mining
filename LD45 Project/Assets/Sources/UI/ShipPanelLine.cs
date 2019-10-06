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
    SHIP_SIZE size;
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
    public SHIP_SIZE Size
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

    public void SetLineInfo(GameObject ship)
    {
        var shipProperties = ship.GetComponent<ShipProperties>();

        this.ship = ship;
        this.size = shipProperties.Size;
        this.price = shipProperties.Price;

        shipNameText.text = ship.name;
        shipSizeText.text = Size.ToString();
        shipPriceText.text = price + "$";

        CheckShipStatus();
        if (!selected)
            RefreshColor();
    }

    private void CheckShipStatus()
    {
        playerShip = ship.GetComponent<ShipPiloting>().PlayerControlled;
        aiShip = ship.GetComponent<ShipPiloting>().AIControlled;
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
        CheckShipStatus();
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
