using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWithEsc : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
        }
    }
}
