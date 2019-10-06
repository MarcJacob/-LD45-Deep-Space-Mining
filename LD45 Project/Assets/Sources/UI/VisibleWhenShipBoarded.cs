using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleWhenShipBoarded : MonoBehaviour
{
    [SerializeField]
    private bool InverseEffect = false;

    private void Awake()
    {
        PlayerInput.OnPlayerShipChanged += PlayerInput_OnPlayerShipChanged;
        
    }

    private void Start()
    {
        PlayerInput_OnPlayerShipChanged(PlayerInput.CurrentShip.gameObject);
    }

    private void PlayerInput_OnPlayerShipChanged(GameObject obj)
    {
        if (obj == null)
        {
            gameObject.SetActive(InverseEffect);
        }
        else
        {
            gameObject.SetActive(!InverseEffect);
        }
    }
}
