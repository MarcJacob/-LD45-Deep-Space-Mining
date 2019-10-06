using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CargoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cargoText;
    [SerializeField]
    private GameObject cargoItemLinePrefab;
    [SerializeField]
    private GameObject panel;

    private Cargo currentCargo;
    private List<TradePanelLine> existingLines = new List<TradePanelLine>();

    private void Awake()
    {
        PlayerInput.OnPlayerShipChanged += PlayerInput_OnPlayerShipChanged;
    }

    private void OnEnable()
    {
        InvokeRepeating("RefreshCargoPanel", 0f, 0.5f);
    }

    private void OnDisable()
    {
        CancelInvoke("RefreshCargoPanel");
    }

    private void PlayerInput_OnPlayerShipChanged(GameObject obj)
    {
        if (obj != null)
        {
            currentCargo = obj.GetComponent<Cargo>();
        }
    }

    private void Update()
    {
        cargoText.text = currentCargo.StoredAmount + " / " + currentCargo.Capacity;
    }

    private void RefreshCargoPanel()
    {

        float lineHeight = cargoItemLinePrefab.GetComponent<RectTransform>().rect.height;
        int lineAmount = 0;
        if (currentCargo != null) lineAmount = currentCargo.GetStoredResourceTypes().Length;

        var delta = panel.GetComponent<RectTransform>().sizeDelta;
        delta.y = lineHeight * lineAmount;
        panel.GetComponent<RectTransform>().sizeDelta = delta;

        if (lineAmount > existingLines.Count)
        {
            int lineAmountToInstantiate = lineAmount - existingLines.Count;
            for (int i = 0; i < lineAmountToInstantiate; i++)
            {
                GameObject newLine = CreateTradeLine(panel, existingLines, lineHeight);
                existingLines.Add(newLine.GetComponent<TradePanelLine>());
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

        if (lineAmount > 0)
        {
            int lineID = 0;
            for (int i = 0; i < currentCargo.StoredResources.Length; i++)
            {
                if (currentCargo.StoredResources[i] > 0)
                {
                    RESOURCE_TYPE t = (RESOURCE_TYPE)i;
                    existingLines[lineID].SetLineInfo(t, currentCargo.StoredResources[i], t.GetBasePrice());
                    lineID++;
                }
            }
        }

    }

    private GameObject CreateTradeLine(GameObject panel, List<TradePanelLine> existingLines, float lineHeight)
    {
        var newLine = Instantiate(cargoItemLinePrefab);
        var linePos = newLine.GetComponent<RectTransform>().position;
        linePos.y = (existingLines.Count + 0.5f) * -lineHeight;

        newLine.GetComponent<RectTransform>().position = linePos;
        newLine.GetComponent<RectTransform>().SetParent(panel.transform, false);
        return newLine;
    }

}
