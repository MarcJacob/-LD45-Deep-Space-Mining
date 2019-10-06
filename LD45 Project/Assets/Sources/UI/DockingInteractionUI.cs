using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingInteractionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject dockingPrompt;
    [SerializeField]
    private GameObject dockedInterface;

    Dockable playerLastShip;
    private bool docked = false;

    private void Awake()
    {
        PlayerInput.OnPlayerShipChanged += (go) => { if (go != null) OnPlayerShipChanged(go.GetComponent<Dockable>()); else OnPlayerShipChanged(null); };
        dockingPrompt.SetActive(false);
    }

    private void OnPlayerShipChanged(Dockable playerShip)
    {
        if (playerLastShip != null)
        {
            playerLastShip.OnShipInDockingRange -= PlayerShip_OnShipInDockingRange;
            playerLastShip.OnShipOutOfDockingRange -= PlayerShip_OnShipOutOfDockingRange;
            playerLastShip.OnShipDocked -= PlayerShip_OnShipDocked;
            playerLastShip.OnShipUndocked -= PlayerShip_OnShipUndocked;
        }

        if (playerShip != null)
        {
            playerShip.OnShipInDockingRange += PlayerShip_OnShipInDockingRange;
            playerShip.OnShipOutOfDockingRange += PlayerShip_OnShipOutOfDockingRange;
            playerShip.OnShipDocked += PlayerShip_OnShipDocked;
            playerShip.OnShipUndocked += PlayerShip_OnShipUndocked;
        }

        playerLastShip = playerShip;
    }

    private void PlayerShip_OnShipUndocked(Dock obj)
    {
        dockedInterface.SetActive(false);
        gameObject.SetActive(true);
        docked = false;

        CameraZoom.Activate();
    }

    private void PlayerShip_OnShipDocked(Dock obj)
    {
        dockingPrompt.SetActive(false);
        gameObject.SetActive(false);
        dockedInterface.GetComponent<ItemTradingUI>().SetDockedStation(obj);
        dockedInterface.GetComponent<ShipTradingUI>().SetDockedStation(obj);
        dockedInterface.SetActive(true);
        docked = true;

        CameraZoom.Deactivate();
    }

    private void PlayerShip_OnShipOutOfDockingRange()
    {
        if (!docked)
            dockingPrompt.SetActive(false);
    }

    private void PlayerShip_OnShipInDockingRange(Dock dock)
    {
        if (!docked)
            dockingPrompt.SetActive(true);
    }

}