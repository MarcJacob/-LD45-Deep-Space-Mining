using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

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
    private float lineHeight;

    private void Awake()
    {
        lineHeight = tradeShipLinePrefab.GetComponent<RectTransform>().rect.height;
    }

    public void SetDockedStation(Dock dock)
    {
        if (this.dock != null)
        {
            this.dock.OnDockableDocked -= OnShipDocked;
        }
        this.dock = dock;
        this.dock.OnDockableDocked += OnShipDocked;
    }

    void OnShipDocked(Dockable dockable)
    {
        // Create line
        var owner = dockable.GetComponent<Ownership>();
        ShipPanelLine line;
        int currentLineAmount = 0;
        if (owner == null || owner.OwnerFactionID == 0)
        {
            // Create line on station's side
            line = CreateLine(dockable.gameObject);
            currentLineAmount = stationTradingPanelLines.Count;
            MoveLineToPanel(stationTradingPanelContent.transform, stationTradingPanelLines, line);

            
        }
        else if (owner != null && owner.OwnerFactionID == 1)
        {
            // Create line on player's side
            line = CreateLine(dockable.gameObject);
            currentLineAmount = playerTradingPanelLines.Count;
            MoveLineToPanel(playerTradingPanelContent.transform, playerTradingPanelLines, line);
        }
        else line = null;

        if (line != null)
        {
            line.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -lineHeight * (currentLineAmount + 0.5f));
            line.SetLineInfo(dockable.gameObject);
            line.gameObject.SetActive(true);

            Action<Dock> handler = null;
            handler = (dock) =>
            {
                dockable.OnShipUndocked -= handler;
                OnShipUndocked(line);
            };
            dockable.OnShipUndocked += handler;
        }
    }

    void OnShipUndocked(ShipPanelLine line)
    {
        RemoveLineFromList(playerTradingPanelLines, line);
        RemoveLineFromList(stationTradingPanelLines, line);
        Destroy(line.gameObject);
    }

    private void RemoveLineFromList(List<ShipPanelLine> list, ShipPanelLine line)
    {
        bool lineRemoved = false;
        for (int l = 0; l < list.Count; l++)
        {
            if (list[l] == line)
            {
                lineRemoved = true;
                
                list.RemoveAt(l);
                l--;
            }
            else if (lineRemoved)
            {
                list[l].GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, lineHeight);
            }
        }
    }

    private ShipPanelLine CreateLine(GameObject ship)
    {
        var line = Instantiate(tradeShipLinePrefab);
        line.SetActive(false);
        line.GetComponent<Button>().onClick.AddListener(() => SelectLine(line));
        return line.GetComponent<ShipPanelLine>();
    }

    private void MoveLineToPanel(Transform panel, List<ShipPanelLine> lines, ShipPanelLine line)
    {
        RectTransform rTransform = line.GetComponent<RectTransform>();
        rTransform.SetParent(panel);
        rTransform.anchoredPosition = new Vector2(0f, (lines.Count + 0.5f) * -lineHeight);
        rTransform.localScale = Vector3.one;
        rTransform.sizeDelta = Vector2.zero;
        lines.Add(line);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, lineHeight * lines.Count);
    }

    public void Sell()
    {
        if (selectedLine != null)
        {
            var ship = selectedLine.Ship;
            uint ownerID = ship.GetComponent<Ownership>() ? ship.GetComponent<Ownership>().OwnerFactionID : 0;

            if (ownerID == 1)
            {
                RemoveLineFromList(playerTradingPanelLines, selectedLine);
                MoveLineToPanel(stationTradingPanelContent.transform, stationTradingPanelLines, selectedLine);
                ship.GetComponent<Ownership>().OwnerFactionID = dock.GetComponent<Ownership>().OwnerFactionID;
                ship.GetComponent<ShipPiloting>().SwitchToNoControl();
                DeselectLines();

                GameManager.AddCash(selectedLine.Price);
            }
            else
            {
                Debug.LogError("Attempted to sell a ship the player doesn't own.");
            }
        }
    }

    public void Buy()
    {
        if (selectedLine != null)
        {
            var ship = selectedLine.Ship;
            uint ownerID = ship.GetComponent<Ownership>() ? ship.GetComponent<Ownership>().OwnerFactionID : 0;

            if (ownerID == 0)
            {
                if (GameManager.PlayerCash < selectedLine.Price) return;

                RemoveLineFromList(stationTradingPanelLines, selectedLine);
                MoveLineToPanel(playerTradingPanelContent.transform, playerTradingPanelLines, selectedLine);
                ship.GetComponent<Ownership>().OwnerFactionID = 1;
                DeselectLines();

                GameManager.RemoveCash(selectedLine.Price);
            }
            else
            {
                Debug.LogError("Attempted to buy a ship that's already owned.");
            }
        }
    }

    public void BuyAIPilot()
    {
        if (selectedLine != null)
        {
            var ship = selectedLine.Ship;
            uint ownerID = ship.GetComponent<Ownership>() ? ship.GetComponent<Ownership>().OwnerFactionID : 0;

            if (ownerID == 1)
            {
                var piloting = ship.GetComponent<ShipPiloting>();
                if (piloting.AIControlled) return;
                else if (GameManager.PlayerCash >= 500)
                {
                    piloting.SwitchToAIControl();
                    GameManager.RemoveCash(500);
                }
            }
            else
            {
                Debug.LogError("Attempted to buy AI pilot for a ship the player doesn't own.");
            }
            DeselectLines();
        }
    }

    public void Board()
    {
        if (selectedLine != null)
        {
            var ship = selectedLine.Ship;
            uint ownerID = ship.GetComponent<Ownership>() ? ship.GetComponent<Ownership>().OwnerFactionID : 0;

            if (ownerID == 1)
            {
                var piloting = ship.GetComponent<ShipPiloting>();
                if (PlayerInput.CurrentShip != null)
                {
                    if (piloting.AIControlled)
                    {
                        PlayerInput.CurrentShip.SwitchToAIControl();
                    }
                    else
                    {
                        PlayerInput.CurrentShip.SwitchToNoControl();
                    }
                }

                piloting.SwitchToPlayerControl();
            }
            else
            {
                Debug.LogError("Attempted to board a ship the player doesn't own.");
            }
            DeselectLines();
        }
    }

    public void SelectLine(GameObject obj)
    {
        var line = obj.GetComponent<ShipPanelLine>();
        if (selectedLine != null) selectedLine.OnLineDeselected();

        selectedLine = line;
        line.OnLineSelected();
    }

    public void DeselectLines()
    {
        List<ShipPanelLine> shipDestroyedLine = new List<ShipPanelLine>();
        foreach (var line in playerTradingPanelLines)
        {
            if (line.Ship != null)
                line.OnLineDeselected();
            else
                shipDestroyedLine.Add(line);
        }
        foreach(var line in shipDestroyedLine)
        {
            RemoveLineFromList(playerTradingPanelLines, line);
            Destroy(line.gameObject);
        }
        shipDestroyedLine.Clear();
        foreach (var line in stationTradingPanelLines)
        {
            if (line.Ship != null)
                line.OnLineDeselected();
            else
                shipDestroyedLine.Add(line);
        }
        foreach (var line in shipDestroyedLine)
        {
            RemoveLineFromList(stationTradingPanelLines, line);
            Destroy(line.gameObject);
        }
    }
}
