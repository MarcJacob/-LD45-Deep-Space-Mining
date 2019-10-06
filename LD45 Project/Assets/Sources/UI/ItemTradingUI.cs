using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System;
using UnityEngine.UI;

public class ItemTradingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerTradingPanelContent;
    [SerializeField]
    private GameObject stationTradingPanelContent;
    [SerializeField]
    private GameObject tradeItemLinePrefab;

    private Cargo playerShipCargo;
    private Cargo stationCargo;
    private Trading stationTradingComponent;

    private List<TradePanelLine> playerTradingPanelLines = new List<TradePanelLine>();
    private List<TradePanelLine> stationTradingPanelLines = new List<TradePanelLine>();
    private float lineHeight;
    private int lineAmount;

    private bool selling;
    TradePanelLine selectedLine;

    public void SetDockedStation(Dock dock)
    {
        stationCargo = dock.GetComponent<Cargo>();
        stationTradingComponent = dock.GetComponent<Trading>(); // TODO handle when station does not have trading component. Do not show the panels.
    }

    private void Awake()
    {
        // Create one line per resource type per panel
        lineHeight = tradeItemLinePrefab.GetComponent<RectTransform>().rect.height;
        lineAmount = Enum.GetValues(typeof(RESOURCE_TYPE)).Length;

        for(int i = 0; i < lineAmount; i++)
        {
            RESOURCE_TYPE t = (RESOURCE_TYPE)i;
            var p = CreateTradeLine(playerTradingPanelContent, playerTradingPanelLines, lineHeight, true);
            p.SetLineInfo(0, 0, 0);
            var o = CreateTradeLine(stationTradingPanelContent, stationTradingPanelLines, lineHeight, false);
            o.SetLineInfo(0, 0, 0);
        }
    }

    private void OnEnable()
    {
        if (PlayerInput.CurrentShip != null)
            playerShipCargo = PlayerInput.CurrentShip.GetComponent<Cargo>();
        PlayerInput.OnPlayerShipChanged += PlayerInput_OnPlayerShipChanged;
        InvokeRepeating("RefreshTradePanels", 0f, 3f);
    }

    private void OnDisable()
    {
        CancelInvoke("RefreshTradePanels");
    }

    private void PlayerInput_OnPlayerShipChanged(GameObject obj)
    {
        if (obj == null) playerShipCargo = null;
        else playerShipCargo = PlayerInput.CurrentShip.GetComponent<Cargo>();
        RefreshTradePanels();
    }

    private void RefreshTradePanels()
    {
        RefreshTradePanel(playerShipCargo, playerTradingPanelLines, true);
        RefreshTradePanel(stationCargo, stationTradingPanelLines, false);
    }

    private void RefreshTradePanel(Cargo cargo, List<TradePanelLine> lines, bool selling)
    {
        if (cargo != null)
        {
            // Pass 1 : update all amounts for all lines.
            var stored = cargo.StoredResources;

            for (int i = 0; i < stored.Length; i++)
            {
                lines[i].SetLineInfo((RESOURCE_TYPE)i, stored[i], ((RESOURCE_TYPE)i).GetBasePrice());
            }

            // Pass 2 : hide all lines that have amount = 0. Position the other ones.
            int visibleLinesCount = 0;
            for (int i = 0; i < stored.Length; i++)
            {
                if (stored[i] > 0)
                {
                    lines[i].gameObject.SetActive(true);

                    var rTransform = lines[i].GetComponent<RectTransform>();
                    var pos = rTransform.localPosition;
                    pos.y = (visibleLinesCount + 0.5f) * -lineHeight;
                    rTransform.localPosition = pos;
                    visibleLinesCount++;
                }
                else
                {
                    lines[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < lineAmount; i++)
            {
                lines[i].gameObject.SetActive(false);
            }
        }
    }

    private TradePanelLine CreateTradeLine(GameObject panel, List<TradePanelLine> lines, float lineHeight, bool selling)
    {
        var newLine = Instantiate(tradeItemLinePrefab);
        var linePos = newLine.GetComponent<RectTransform>().position;
        linePos.y = (lines.Count + 0.5f) * -lineHeight;

        newLine.GetComponent<RectTransform>().position = linePos;
        newLine.GetComponent<RectTransform>().SetParent(panel.transform, false);

        newLine.GetComponent<Button>().onClick.AddListener(() => SelectLine(newLine, selling));
        var trade = newLine.GetComponent<TradePanelLine>();
        lines.Add(trade);
        return trade;
    }

    public void Sell()
    {
        int amount = 1;
        if (Input.GetKey(KeyCode.LeftShift)) amount = 10;
        else if (Input.GetKey(KeyCode.LeftControl)) amount = 100;
        if (selling && selectedLine != null)
        {
            stationTradingComponent.BuyFrom(playerShipCargo, (RESOURCE_TYPE)selectedLine.ResourceID, (uint)amount, selectedLine.PricePerUnit, true);
            RefreshTradePanels();
        }
    }

    public void Buy()
    {
        int amount = 1;
        if (Input.GetKey(KeyCode.LeftShift)) amount = 10;
        else if (Input.GetKey(KeyCode.LeftControl)) amount = 100;
        if (!selling && selectedLine != null)
        {
            stationTradingComponent.SellTo(playerShipCargo, (RESOURCE_TYPE)selectedLine.ResourceID, (uint)amount, selectedLine.PricePerUnit, true);
            RefreshTradePanels();
        }
    }

    public void SelectLine(GameObject obj, bool selling)
    {
        var line = obj.GetComponent<TradePanelLine>();
        if (selectedLine != null) selectedLine.OnLineDeselected();

        selectedLine = line;
        this.selling = selling;
        line.OnLineSelected();
    }
}
