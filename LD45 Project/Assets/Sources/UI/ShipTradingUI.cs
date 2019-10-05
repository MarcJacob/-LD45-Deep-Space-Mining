using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShipTradingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerTradingPanelContent;
    [SerializeField]
    private GameObject stationTradingPanelContent;
    [SerializeField]
    private GameObject tradeShipLinePrefab;

    private List<ShipPanelLine> playerTradingPanelLines = new List<ShipPanelLine>();
    private List<ShipPanelLine> stationTradingPanelLines = new List<ShipPanelLine>();

    private bool selling;
    ShipPanelLine selectedLine;
    private Dock dock;

    public void SetDockedStation(Dock dock)
    {
        this.dock = dock;
    }

    private void OnEnable()
    {
        InvokeRepeating("RefreshShipPanels", 0f, 3f);
    }

    private void OnDisable()
    {
        CancelInvoke("RefreshShipPanels");
    }

    private void RefreshShipPanels()
    {
        RefreshShipPanel(playerTradingPanelContent, playerTradingPanelLines, true);
        RefreshShipPanel(stationTradingPanelContent, stationTradingPanelLines, false);
    }

    private void RefreshShipPanel(GameObject panel, List<ShipPanelLine> existingLines, bool selling)
    {
        IEnumerable<GameObject> shipsInList;
        if (selling)
        {
            shipsInList = GetPlayerShipsAtDock();
        }
        else
        {
            shipsInList = GetUnownedShipsAtDock();
        }

        float lineHeight = tradeShipLinePrefab.GetComponent<RectTransform>().rect.height;
        int lineAmount = shipsInList.Count();

        var delta = panel.GetComponent<RectTransform>().sizeDelta;
        delta.y = lineHeight * lineAmount;
        panel.GetComponent<RectTransform>().sizeDelta = delta;

        if (lineAmount > existingLines.Count)
        {
            int lineAmountToInstantiate = lineAmount - existingLines.Count;
            for (int i = 0; i < lineAmountToInstantiate; i++)
            {
                GameObject newLine = CreateShipLine(panel, existingLines, lineHeight, selling);
                existingLines.Add(newLine.GetComponent<ShipPanelLine>());
            }
        }
        else if (lineAmount < existingLines.Count)
        {
            for (int i = lineAmount; i < existingLines.Count; i++)
            {
                var line = existingLines[i];
                existingLines.RemoveAt(i);
                Destroy(line.gameObject);
            }
        }

        int lineID = 0;
        foreach (var go in shipsInList)
        {
            existingLines[lineID].SetLineInfo(go, 1, 1000f);
            lineID++;
        }
    }

    private IEnumerable<GameObject> GetUnownedShipsAtDock()
    {
        var dockedShips = dock.DockedShips;
        var unownedShips = dockedShips.Where(ship => ship.GetComponent<Ownership>().OwnerFactionID == 0);

        return unownedShips.Select(s => s.gameObject);
    }

    private IEnumerable<GameObject> GetPlayerShipsAtDock()
    {
        var dockedShips = dock.DockedShips;
        var playerShips = dockedShips.Where(ship => ship.GetComponent<Ownership>().OwnerFactionID == 1);

        return playerShips.Select(s => s.gameObject);
    }

    private GameObject CreateShipLine(GameObject panel, List<ShipPanelLine> existingLines, float lineHeight, bool selling)
    {
        var newLine = Instantiate(tradeShipLinePrefab);
        var linePos = newLine.GetComponent<RectTransform>().position;
        linePos.y = (existingLines.Count + 0.5f) * -lineHeight;

        newLine.GetComponent<RectTransform>().position = linePos;
        newLine.GetComponent<RectTransform>().SetParent(panel.transform, false);

        newLine.GetComponent<Button>().onClick.AddListener(() => SelectLine(newLine, selling));

        return newLine;
    }

    public void Sell()
    {
        if (selling && selectedLine != null)
        {
            GameManager.AddCash((int)selectedLine.Price);
            var ship = selectedLine.Ship;
            ship.GetComponent<Ownership>().OwnerFactionID = 0;
            RefreshShipPanels();
        }
    }

    public void Buy()
    {
        if (!selling && selectedLine != null)
        {
            int totalCost = (int)selectedLine.Price;
            if (GameManager.PlayerCash >= totalCost)
            {
                GameManager.RemoveCash(totalCost);
                var ship = selectedLine.Ship;

                ship.GetComponent<Ownership>().OwnerFactionID = 1;
                DeselectLine();
                RefreshShipPanels();
            }
        }
    }

    public void BuyAIPilot()
    {
        if (selling && selectedLine != null)
        {
            int totalCost = 500;
            if (GameManager.PlayerCash >= totalCost)
            {
                GameManager.RemoveCash(totalCost);
                var ship = selectedLine.Ship;
                ship.GetComponent<ShipPiloting>().SwitchToAIControl();
                DeselectLine();
                RefreshShipPanels();
            }
        }
    }

    public void Board()
    {
        if (selling && selectedLine != null)
        {
            var ship = selectedLine.Ship;
            var previousShip = PlayerInput.CurrentShip;
            if (previousShip != null)
            {
                if (ship.GetComponent<ShipPiloting>().AIControlled)
                {
                    previousShip.GetComponent<ShipPiloting>().SwitchToAIControl();
                }
                else
                    previousShip.GetComponent<ShipPiloting>().SwitchToNoControl();
            }
            ship.GetComponent<ShipPiloting>().SwitchToPlayerControl();
            DeselectLine();
            RefreshShipPanels();
        }
    }

    public void SelectLine(GameObject obj, bool selling)
    {
        var line = obj.GetComponent<ShipPanelLine>();
        if (selectedLine != null) selectedLine.OnLineDeselected();

        selectedLine = line;
        this.selling = selling;
        line.OnLineSelected();
    }

    public void DeselectLine()
    {
        if (selectedLine != null) selectedLine.OnLineDeselected();
    }
}
