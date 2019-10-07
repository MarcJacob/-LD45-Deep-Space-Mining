using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnNetworthReached : MonoBehaviour
{
    [SerializeField]
    private float netWorth;
    [SerializeField]
    GameObject[] activated;

    bool activatedBefore = false;

    private void Awake()
    {
        
    }

    void Update()
    {
        if (!activatedBefore && GameManager.NetWorth >= netWorth)
        {
            foreach(var go in activated)
            {
                go.SetActive(true);
            }
            activatedBefore = true;
        }
    }
}
