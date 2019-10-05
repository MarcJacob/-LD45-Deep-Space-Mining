using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleWhenShipBoarded : MonoBehaviour
{
    private void Awake()
    {
        PlayerInput.OnPlayerShipChanged += PlayerInput_OnPlayerShipChanged;
    }

    private void PlayerInput_OnPlayerShipChanged(GameObject obj)
    {
        if (obj == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
